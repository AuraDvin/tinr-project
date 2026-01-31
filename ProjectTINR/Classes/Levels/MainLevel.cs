using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.UI;
using ProjectTINR.Classes.NPCs;
using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;
namespace ProjectTINR.Classes.Levels;


public class MainLevel : Level {
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

        if (!_isResetting) {
            if (player != null && player.Health <= 0) {
                if (player.LastCheckpoint != Vector2.Zero) {
                    _pendingRespawn = player.LastCheckpoint;
                }
                _isResetting = true;
                Reset();
                _isResetting = false;
            }
        }
    }

    public override void Reset() {
        base.Reset();
        // Console.WriteLine("Main Level reset.");
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
    }
}

