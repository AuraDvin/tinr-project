using System;
using System.IO;
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

    public void ReadData(string jsonPath) {
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

        JsonObject playerObj = levelDataObj["player"].AsObject();
        _sceneData.Add(
            new Player((int)playerObj["health"], Game) {
                Position = new(
                (float)playerObj["spawnPosition"].AsObject()["x"],
                (float)playerObj["spawnPosition"].AsObject()["y"]
            ),
            });

        JsonArray enemiesJson = levelDataObj["enemies"].AsArray();
        foreach (var enemyj in enemiesJson) {
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

        JsonArray platformsArr = levelDataObj["platforms"].AsArray();
        foreach(var platformj in platformsArr) {
            JsonObject a = platformj.AsObject();
            Vector2 pos = new(
                (float)a["rect"].AsObject()["x"],
                (float)a["rect"].AsObject()["y"]
            );
            int w = (int)a["rect"].AsObject()["w"];
            int h = (int)a["rect"].AsObject()["h"];

            _sceneData.Add(new Floor(Game, pos, w, h));
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
