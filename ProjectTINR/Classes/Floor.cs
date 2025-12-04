using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes;

public class Floor : GameObject, IPhysicsObject, IDrawableGameComponent {
    public Floor(Game game) : base(game) {
    }

    public CollisionShapeType CollisionType { get => CollisionShapeType.StaticRectangle; set => throw new NotImplementedException(); }
    public Vector2 Velocity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Vector2 Position { get => new(0, 400); set => throw new NotImplementedException(); }

    public override void Initialize() {
    }

    public override void Update(GameTime gameTime) {
    }
}
