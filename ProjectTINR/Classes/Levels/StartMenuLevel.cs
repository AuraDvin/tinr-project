using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Levels;

public class StartMenuLevel : Level {
    public StartMenuLevel(Game game) : base(game) {
        _scene = [];
        Content = game.Content;
        IGameComponent player = new Player(game);
        // Player player2 = new Player(game);
        // player2.Position = new Vector2(200, -400);

        Floor floor = new Floor(game);
        _scene.Add(floor);
        // IPhysicsObject physicsObject = new IPhysicsObject {
        //     CollisionType = CollisionShapeType.Rectangle
        // };
        _scene.Add(player);
        // _scene.Add(player2);
        game.Components.Add(player);
        // game.Components.Add(player2);
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
