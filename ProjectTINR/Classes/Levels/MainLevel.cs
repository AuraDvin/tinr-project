using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.UI;
using ProjectTINR.Classes.NPCs;
using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;
namespace ProjectTINR.Classes.Levels;


public class MainLevel : Level {
    // private Vector2 _playerSpawnPosition = new(0f, 0f);
    // private Vector2 _enemySpawnPosition = new(500f, 0f);
    // private Vector2 _flyingEnemySpawnPosition = new(-800f, -100f);
    private CameraObject _camera;
    public MainLevel(Game game) : base(game) {
        _scene = [];
        _uiScene = [];
    }
    public override void Initialize() {
        Player player;
        int loadAttempts = 0;
        while (!LevelDataManager.Instance.LoadData(this)) {

            // Floor floor = new(Game, new(-100, 400), 400, 50);
            // Floor floor2 = new(Game, new(500, 200), 400, 50);
            // Floor floor3 = new(Game, new(-500, 200), 400, 50);
            // Floor floor4 = new(Game, new(0, 0), 400, 50);

            // player = new(Game) {
            //     Position = _playerSpawnPosition
            // };
            // StationaryEnemy se = new(Game) {
            //     Position = _enemySpawnPosition,
            //     Scene = Scene
            // };
            // FlyingEnemy fe = new(Game) {
            //     Position = _flyingEnemySpawnPosition,
            //     Scene = Scene
            // };

            // CameraObject camera = new(Game) {
            //     Position = new Vector2(0, 0),
            //     Zoom = 1.0f
            // };

            // _scene.Add(floor2);
            // _scene.Add(floor);
            // _scene.Add(floor3);
            // _scene.Add(floor4);
            // _scene.Add(player);
            // _scene.Add(se);
            // _scene.Add(fe);
            // _scene.Add(camera);

            // foreach (IGameComponent obj in _scene) {
            //     Game.Components.Add(obj);
            // }
            int levelNum = GameSettings.LevelNum;
            string levelPath = $"Content/levels/level{levelNum}.json";

            Console.WriteLine($"trying to load level {levelPath}");
            LevelDataManager.Instance.ReadData(levelPath);
            loadAttempts++;
            if (loadAttempts >= 10) {
                throw new Exception("Failed to load level data!");
            }
        }

        player = Scene.FindByType<Player>() ?? throw new Exception("player is gone in level reserilization");
        UIHealthElement he = new(Game, "", "") {
            Player = player
        };

        _camera = Scene.FindByType<CameraObject>();
        if (_camera == null) {
            _camera = new CameraObject(Game);
            Scene.Add(_camera);
        }
        
        foreach (GameObject obj in _scene) {
            if (obj is ISceneManipulator sceneManipulator) {
                sceneManipulator.Scene = _scene;
            }
        }

        foreach (GameObject obj in _uiScene) {
            if (obj is ISceneManipulator sceneManipulator) {
                sceneManipulator.Scene = _scene;
            }
        }


        _uiScene.Add(he);

        Console.WriteLine("Main Level initialized.");
    }
    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        foreach(GameObject obj in _scene) {
            obj.Update(gameTime);
        }
        foreach(GameObject obj in _uiScene) {
            obj.Update(gameTime);
        }
    }

    public override void Reset() {
        base.Reset();
        Console.WriteLine("Main Level reset.");
        LevelDataManager.Instance.RemoveData();
        Initialize();
    }


    public override void Serialize() {
        LevelDataManager.Instance.SaveData(this);
        // Player should have the health, position etc 

        // Enemies should be saved positions, state of attack (if applicable)

        // Projectiles should be retained 

        // Can we just save the scene array object? 

    }
}

