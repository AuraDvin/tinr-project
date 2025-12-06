using System;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.Physics.Shapes;

public class RectCollisionShape(bool isStatic) : ICollisionShape, IMoveComponent {
    protected Rectangle _rectangle = new(0, 0, 128, 128);
    protected Vector2 _velocity;
    private readonly bool _isStatic = isStatic;
    public bool ShouldSimulate { get => !_isStatic; }
    public virtual Vector2 Position {
        get => new(_rectangle.X, _rectangle.Y);
        set {
            _rectangle.X = (int)value.X;
            _rectangle.Y = (int)value.Y;
        }
    }

    public Rectangle Rectangle { get => _rectangle; set => _rectangle = value; }
    public Vector2 Velocity {
        get => _velocity;
        set {
            if (_isStatic)
                return;
            _velocity = value;
        }
    }

    public virtual bool OnCollision(ICollisionShape other) {
        Console.WriteLine("Also bad bad bad");
        return ShouldSimulate;
    }

    public virtual void Update(GameTime gameTime) {
    }

    public virtual void Initialize() {
    }
}
