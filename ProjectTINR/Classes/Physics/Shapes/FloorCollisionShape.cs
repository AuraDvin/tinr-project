using System;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Physics.Shapes;

public class FloorCollisionShape : RectCollisionShape {
    public FloorCollisionShape() : base(true) {
        _rectangle = new(0, 400, 1200, 50);
    }
    public FloorCollisionShape(int x, int y, int width, int height) : base(true) {
        _rectangle = new(x, y, width, height);
    }

    public FloorCollisionShape(Microsoft.Xna.Framework.Rectangle rectangle) : base(true) {
        _rectangle = rectangle;
    }

    public override bool OnCollision(ICollisionShape other) {
        // Console.WriteLine("FloorCollisionShape OnCollision called.");
        // Static objects should not resolve collisions
        // Non-static objects have to handle collision with a static object themselves
        return false;
    }
}
