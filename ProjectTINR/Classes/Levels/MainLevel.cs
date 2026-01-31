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
        int levelNum = GameSettings.LevelNum;
        string levelPath = $"Content/levels/level{levelNum}.json";


        while (!LevelDataManager.Instance.LoadData(this)) {

            Console.WriteLine($"trying to load level {levelPath}");

            if (_isResetting) {
                LevelDataManager.Instance.ReadData(levelPath, levelNum);
            }
            else {
                LevelDataManager.Instance.ReadData(levelPath);
            }

            loadAttempts++;
            if (loadAttempts >= 10) {
                throw new Exception("Failed to load level data!");
            }
        }

        player = Scene.FindByType<Player>() ?? throw new Exception("player is gone in level reserilization");

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


        UIHealthElement he = new(Game, "", "") {
            Player = player
        };

        if (_uiScene.FindByType<UIHealthElement>() != null) {
            _uiScene.RemoveByType<UIHealthElement>();
        }
        _uiScene.Add(he);

        Console.WriteLine("Main Level initialized.");
    }
    private Vector2? _pendingRespawn = null;
    private bool _isResetting = false;

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        foreach (GameObject obj in _scene) {
            obj.Update(gameTime);
        }
        foreach (GameObject obj in _uiScene) {
            obj.Update(gameTime);
        }
        Player player = Scene.FindByType<Player>();
        //        if (player.Health <= 0) {

        // }
        // Check for player death and trigger reset that will respawn at last checkpoint
        if (!_isResetting) {
            if (player != null && player.Health <= 0) {
                Console.WriteLine("Player died - respawning at last checkpoint or level start.");
                if (player.LastCheckpoint != Vector2.Zero) {
                    _pendingRespawn = player.LastCheckpoint;
                }
                // else {
                //     _pendingRespawn = null; // no checkpoint collected, use level start
                // }
                _isResetting = true;
                Reset();
                _isResetting = false;
            }
        }
    }

    public override void Reset() {
        base.Reset();
        Console.WriteLine("Main Level reset.");
        LevelDataManager.Instance.RemoveData();
        Initialize();

        // Apply pending respawn position if set
        if (_pendingRespawn.HasValue) {
            Player player = Scene.FindByType<Player>();
            if (player != null) {
                player.Position = _pendingRespawn.Value;
                player.LastCheckpoint = _pendingRespawn.Value;
            }
            _pendingRespawn = null;
        }
    }


    public override void Serialize() {
        LevelDataManager.Instance.SaveData(this);
        // Player should have the health, position etc 

        // Enemies should be saved positions, state of attack (if applicable)

        // Projectiles should be retained 

        // Can we just save the scene array object? 

    }
}

