using System;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.Physics.Shapes;

public class PlayerCollisionShape : RectCollisionShape {
    Vector2 _offset = new(50, 0);

    bool _onFloor = false;
    public bool OnFloor { get => _onFloor; set => _onFloor = value; }

    public override Vector2 Position {
        get => new Vector2(_rectangle.X, _rectangle.Y) - _offset;
        set {
            _rectangle.X = (int)(value.X + _offset.X);
            _rectangle.Y = (int)(value.Y + _offset.Y);
        }
    }
    public PlayerCollisionShape() : base(false) {
        _rectangle = new Rectangle(0, 0, 194/2, 194);
    }

    public override bool OnCollision(ICollisionShape other) {
        Console.WriteLine("PlayerCollisionShape OnCollision called.");
        // Static objects should not resolve collisions
        // Non-static objects have to handle collision with a static object themselves
        // This should be Floor collision class rather than the Class for the floor object
        if (other is FloorCollisionShape) {
        //     Console.WriteLine("Player collided with Floor. Setting vertical velocity to 0 if positive.");
            Velocity = new(Velocity.X, Math.Min(Velocity.Y, 0)); 
            _onFloor = true;
            return false;
        }

        return true;
    }
}
