using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Levels;

public class StartMenuLevel : Level {
    public StartMenuLevel(Game game) : base(game) {
        _scene = [];
        Content = game.Content;

        Floor floor = new Floor(game, new(0, 400));
        _scene.Add(floor);

        Floor floor2 = new Floor(game, new(500, 200));
        _scene.Add(floor2);

        Player player = new Player(game);
        _scene.Add(player);
        game.Components.Add(player);

        player.Position = new Vector2(0, 0);

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
