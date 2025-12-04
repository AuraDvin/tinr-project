using System;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.Physics.Shapes;

public class RectCollisionShape(bool _static) : ICollisionShape, IMoveComponent {
    protected Rectangle _rectangle = new(0, 0, 500, 5040);
    protected Vector2 _velocity;
    private readonly bool _isStatic = _static;
    public bool ShouldSimulate { get => !_isStatic; }
    public Vector2 Position {
        get => new(_rectangle.Center.X, _rectangle.Center.Y);
        set {
            _rectangle.X = (int)(value.X - _rectangle.Width / 2.0);
            _rectangle.Y = (int)(value.Y - _rectangle.Height / 2.0);
        }
    }

    public Rectangle Rectangle { get => _rectangle; set => _rectangle = value; }
    public Vector2 Velocity { get => _velocity; set => _velocity = value; }

    public void Initialize() {
    }

    public void Update(GameTime gameTime) {
    }

    public bool OnCollision(ICollisionShape other) {
        if (!_isStatic)
            return true;
        // Static objects should not resolve collisions
        // Non-static objects have to handle collision with a static object themselves
        return false;
    }
}
