using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ProjectTINR.Classes.Levels;

public class StartMenuLevel : Level {
    public StartMenuLevel(Game game) : base(game) {
        _scene = [];
        Content = game.Content;
        IGameComponent player = new Player(game);
        _scene.Add(player);
        game.Components.Add(player);
        Console.WriteLine("Start Menu Level initialized.");
    }

    public ContentManager Content { get; }

    public override void Initialize() {
        base.Initialize();
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
    }
}
