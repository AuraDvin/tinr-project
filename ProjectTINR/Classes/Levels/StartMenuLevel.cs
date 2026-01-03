using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.NPCs;
using ProjectTINR.Classes.Objects;

namespace ProjectTINR.Classes.Levels;

public class StartMenuLevel : Level {
    private Vector2 _playerSpawnPosition = new(0f, 0f);
    private Vector2 _enemySpawnPosition = new(500f, 0f);
    private Vector2 _flyingEnemySpawnPosition = new(-500f, 0f);
    public StartMenuLevel(Game game) : base(game) {
        _scene = [];
    }
    public override void Initialize() {
        Floor floor = new(Game, new(0, 400));
        Floor floor2 = new(Game, new(500, 200));
        
        Player player = new(Game) {
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

        _scene.Add(floor2);
        _scene.Add(floor);
        _scene.Add(player);
        _scene.Add(se);
        _scene.Add(fe);

        foreach (IGameComponent obj in _scene) {
            Game.Components.Add(obj);
        }
        Console.WriteLine("Start Menu Level initialized.");
        base.Initialize();
    }
    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
    }
    public override void Reset() {
        Player player = _scene.FindByType<Player>();
        player.Position = _playerSpawnPosition;
        Console.WriteLine("Start Menu Level reset.");
        base.Reset();
    }
}
