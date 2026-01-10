using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.UI;
using ProjectTINR.Classes.NPCs;
using ProjectTINR.Classes.Objects;using ProjectTINR.Classes.UI;
namespace ProjectTINR.Classes.Levels;


public class MainLevel : Level {
    private Vector2 _playerSpawnPosition = new(0f, 0f);
    private Vector2 _enemySpawnPosition = new(500f, 0f);
    private Vector2 _flyingEnemySpawnPosition = new(-500f, 0f);
    public MainLevel(Game game) : base(game) {
        _scene = [];
        _uiScene = [];
    }
    public override void Initialize() {
        Player player;
        if (!LevelDataManager.LoadData(this)){
            Floor floor = new(Game, new(0, 400));
            Floor floor2 = new(Game, new(500, 200));
            
            player = new(Game) {
                Position = _playerSpawnPosition
            };
            StationaryEnemy se = new(Game) {
                Position = _enemySpawnPosition,
                Scene = Scene
            };
            FlyingEnemy fe = new(Game) {
            Position = _flyingEnemySpawnPosition,
            Scene = Scene
            };

            CameraObject camera = new(Game) {
                Position = new Vector2(0, 0),
                Zoom = 1.0f
            };

            _scene.Add(floor2);
            _scene.Add(floor);
            _scene.Add(player);
            _scene.Add(se);
            _scene.Add(fe);
            _scene.Add(camera);

            foreach (IGameComponent obj in _scene) {
                Game.Components.Add(obj);
            }
            
        } else {
            player = Scene.FindByType<Player>() ?? throw new Exception("player is gone in level reserilization");
        }

        UIHealthElement he = new(Game) {
            Player = player
        };

        _uiScene.Add(he);

        Console.WriteLine("Main Level initialized.");
    }
    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
    }
    public override void Reset() {
        base.Reset();
        Console.WriteLine("Main Level reset.");
        LevelDataManager.RemoveData();
        Initialize();
    }


    public override void Serialize() {
        LevelDataManager.SaveData(this);
        // Player should have the health, position etc 

        // Enemies should be saved positions, state of attack (if applicable)
        
        // Projectiles should be retained 
        
        // Can we just save the scene array object? 

    }
}

