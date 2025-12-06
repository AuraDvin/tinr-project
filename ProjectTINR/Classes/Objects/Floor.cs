using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes;

public class Floor : GameObject, IStaticPhysicsObject, IDrawableGameComponent {
    protected override string _prefix => "Floor";
    private readonly Vector2 _position = new Vector2(0, 400);
    public Floor(Game game, Vector2 position) : base(game) {
        _position = position;
    }

    public CollisionShapeType CollisionType { get => CollisionShapeType.FloorCollisionShape; set => throw new NotImplementedException(); }
    public Vector2 Position { get => _position; set { } }

    public override void Initialize() {
        base.Initialize();
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
    }
}
