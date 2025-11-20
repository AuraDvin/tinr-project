using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Levels;

public class StartMenuLevel : Level {
    public StartMenuLevel(Game game) : base(game) {
        _scene = [];
        Content = game.Content;

        // GameObject Player = new GameObject(Game);
        IGameComponent Player = new Player(game);
        _scene.Add(Player);

        Console.WriteLine("Start Menu Level initialized.");
    }

    public ContentManager Content { get; }

    public override void Initialize() {
        base.Initialize();
    }

    public override void Update(GameTime gameTime) {
        foreach (IGameComponent obj in _scene) {
            if (obj is IUpdatableGameComponent) {
                // Console.WriteLine("Updating object in StartMenuLevel.");
                ((IUpdatableGameComponent)obj).Update(gameTime);
            }
        }
        base.Update(gameTime);
    }
}
