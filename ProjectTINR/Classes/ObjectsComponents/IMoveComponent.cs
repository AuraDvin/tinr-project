using ProjectTINR.Classes.ObjectsComponents;

using Microsoft.Xna.Framework;
using System;

namespace ProjectTINR.Classes;

public interface IMoveComponent : IPositionComponent, IUpdatableGameComponent
{
    public Vector2 Velocity { get; set; }

    public new void Update(GameTime gameTime)
    {
        Console.WriteLine("MoveableComp - Update called");
        if (Velocity.LengthSquared() < 0.25f) {
            Velocity = new(0,0);
        }
        if (Velocity.LengthSquared() > 640000.0f) {
            Velocity.Normalize();
            Velocity *= 800.0f;
        }
        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}
