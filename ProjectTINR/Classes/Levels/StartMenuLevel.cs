using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Levels;

public class StartMenuLevel : Level {
    private Vector2 _playerSpawnPosition = new(0f,0f);
    public StartMenuLevel(Game game) : base(game) {
        _scene = [];
    }

    public ContentManager Content { get; }

    public override void Initialize() {
        Floor floor = new(Game, new(0, 400));
        Floor floor2 = new(Game, new(500, 200));
        Player player = new(Game) {
            Position = _playerSpawnPosition
        };

        _scene.Add(floor2);
        _scene.Add(floor);
        _scene.Add(player);

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
        base.Reset();
    }
}
