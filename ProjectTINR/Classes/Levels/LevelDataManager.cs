using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.NPCs;
using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Levels;

public class LevelDataManager {
    private static LevelDataManager _instance;
    public ProjectTinr Game { get; set; } = null;
    public static LevelDataManager Instance {
        get {
            _instance ??= new();
            return _instance;
        }
        set { }
    }

    private LevelDataManager() {
    }

    private Scene _sceneData = null;

    // Tracks collected pickups (including checkpoints) per level so they do not respawn after a reset
    private readonly Dictionary<int, HashSet<string>> _collectedPickups = new();

    private static string PickupKey(PickupType type, Vector2 pos) => $"{type}:{(int)pos.X}:{(int)pos.Y}";

    public void MarkPickupCollected(int levelNum, PickupType type, Vector2 pos) {
        if (!_collectedPickups.ContainsKey(levelNum)) {
            _collectedPickups[levelNum] = new HashSet<string>();
        }
        _collectedPickups[levelNum].Add(PickupKey(type, pos));
    }

    public bool IsPickupCollected(int levelNum, PickupType type, Vector2 pos) {
        if (!_collectedPickups.ContainsKey(levelNum)) return false;
        return _collectedPickups[levelNum].Contains(PickupKey(type, pos));
    }

    public void ClearCollectedPickups(int levelNum) {
        if (_collectedPickups.ContainsKey(levelNum)) _collectedPickups.Remove(levelNum);
    }

    public void SaveData(Level level) {
        GameObject[] arr = new GameObject[level.Scene.Count];
        level.Scene.CopyTo(arr);
        _sceneData = [.. arr];
    }

    public void SaveToFile() {
        string path = GetLevelDataFilePath();
        // TODO - pseudo code:
        // File file = new(path) 
        // file.write(_sceneData)
        // file.close()
    }

    public void ReadFromFile() {
        string path = GetLevelDataFilePath();
        // TODO - pseudo code:
        // File file = new(path) 
        // _sceneData = file.read()
        // file.close()
    }

    public bool LoadData(Level level) {
        if (_sceneData != null) {
            GameObject[] arr = new GameObject[_sceneData.Count];
            _sceneData.CopyTo(arr);
            level.Scene = [.. arr];
            return true;
        }
        return false;
    }

    public void RemoveData() {
        _sceneData = null;
    }

    public void ReadData(string jsonPath){
        ReadData(jsonPath, -1);
    }

    public void ReadData(string jsonPath, int levelNum) {
        _sceneData = new();
        string jsonString = File.ReadAllText(jsonPath);
        JsonNode levelDataNode = JsonNode.Parse(jsonString);
        JsonObject levelDataObj = levelDataNode.AsObject() ?? throw new Exception("Couldn't convert level data to json object");
        if (
            !levelDataObj.ContainsKey("player") ||
            !levelDataObj.ContainsKey("checkpoints") ||
            !levelDataObj.ContainsKey("enemies") ||
            !levelDataObj.ContainsKey("pickups") ||
            !levelDataObj.ContainsKey("platforms")) {
            throw new Exception("Json is missing a required field");
        }

        JsonArray enemiesArr = levelDataObj["enemies"].AsArray();
        JsonArray platformsArr = levelDataObj["platforms"].AsArray();
        JsonArray checkpointsArr = levelDataObj["checkpoints"].AsArray();
        JsonArray pickupsArr = levelDataObj["pickups"].AsArray();

        JsonObject playerObj = levelDataObj["player"].AsObject();
        _sceneData.Add(
            new Player((int)playerObj["health"], Game) {
                Position = new(
                (float)playerObj["spawnPosition"].AsObject()["x"],
                (float)playerObj["spawnPosition"].AsObject()["y"]
            ),
            });


        foreach (var enemyj in enemiesArr) {
            JsonObject a = enemyj.AsObject();
            Vector2 positon = new(
                (float)a["position"].AsObject()["x"],
                (float)a["position"].AsObject()["y"]
            );

            switch ((string)a["type"]) {
                case "stationary":
                    _sceneData.Add(new StationaryEnemy(Game) {
                        Position = positon,
                    });
                    break;
                case "flying":
                    _sceneData.Add(new FlyingEnemy(Game) {
                        Position = positon
                    });
                    break;
                default:
                    throw new Exception($"unknown enemy type {(string)a["type"]}");
            }
        }

        foreach (var platformj in platformsArr) {
            JsonObject a = platformj.AsObject();
            Vector2 pos = new(
                (float)a["rect"].AsObject()["x"],
                (float)a["rect"].AsObject()["y"]
            );
            int w = (int)a["rect"].AsObject()["w"];
            int h = (int)a["rect"].AsObject()["h"];

            _sceneData.Add(new Floor(Game, pos, w, h));
        }

        foreach (JsonObject pickupj in pickupsArr) {
            PickupObject pickup;
            Vector2 pos = new(
                (float)pickupj["position"]["x"],
                (float)pickupj["position"]["y"]
            );

            string t = (string)pickupj["type"];
            PickupType ptype;
            switch (t) {
                case "big":
                    ptype = PickupType.BIGGER_PROJECTILE;
                    break;
                case "fast":
                    ptype = PickupType.SHOOT_SPEED;
                    break;
                case "heal":
                    ptype = PickupType.HEAL;
                    break;
                default:
                    throw new Exception("unknown pickup type");
            }

            if (IsPickupCollected(levelNum, ptype, pos)) {
                // Skip pickups already collected
                continue;
            }

            switch (t) {
                case "big":
                    pickup = new BiggerProjectilePickup(Game);
                    break;
                case "fast":
                    pickup = new ShootFasterPickup(Game);
                    break;
                case "heal":
                    pickup = new HealPickup(Game);
                    break;
                default:
                    throw new Exception("unknown pickup type");
            }
            pickup.Position = pos;
            _sceneData.Add(pickup);
        }

        for (int i = 0; i < checkpointsArr.Count; i++) {
            Vector2 pos = new(
                (float)checkpointsArr[i]["position"]["x"],
                (float)checkpointsArr[i]["position"]["y"]
            );
            if (IsPickupCollected(levelNum, PickupType.CHECKPOINT, pos)) {
                continue;
            }
            _sceneData.Add(new Checkpoint(Game, pos, i == checkpointsArr.Count - 1));
        }
    }
    private static string s_fileName = "data";
    private static string GetLevelDataFilePath() {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string dataDir = Path.Combine(appDataPath, "enchanted");

        if (!Directory.Exists(dataDir)) {
            Directory.CreateDirectory(dataDir);
        }

        return Path.Combine(dataDir, s_fileName);
    }
}
