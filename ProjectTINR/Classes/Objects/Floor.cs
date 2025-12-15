using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics;

namespace ProjectTINR.Classes.Objects;

public class Floor(Game game, Vector2 position) : GameObject(game), IStaticPhysicsObject, IDrawableGameComponent {
    protected override string _prefix => "Floor";
    private readonly Vector2 _position = position;

    public CollisionShapeType CollisionType { get => CollisionShapeType.FloorCollisionShape; set => throw new NotImplementedException(); }
    public Vector2 Position { get => _position; set { } }

    public override void Initialize() {
        base.Initialize();
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
    }
}
