using System;
using System.Numerics;

namespace ProjectTINR.Classes.Physics.Shapes;

public class PlayerCollisionShape : RectCollisionShape {
    public PlayerCollisionShape() : base(false) {
    }

    public new bool OnCollision(ICollisionShape other) {
        // Static objects should not resolve collisions
        // Non-static objects have to handle collision with a static object themselves
        if (other is Floor) {
            Velocity = new(Velocity.X, 0); 
        }
        return false;
    }
}
