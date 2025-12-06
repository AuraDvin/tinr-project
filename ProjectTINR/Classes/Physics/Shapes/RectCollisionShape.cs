using System;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.Physics.Shapes;

public class RectCollisionShape(bool _static) : ICollisionShape, IMoveComponent {
    protected Rectangle _rectangle = new(0, 0, 128, 128);
    protected Vector2 _velocity;
    private readonly bool _isStatic = _static;
    public bool ShouldSimulate { get => !_isStatic; }
    public virtual Vector2 Position {
        get => new(_rectangle.X, _rectangle.Y);
        set {
            // if (_isStatic)
            //     return;
            // _rectangle.X = (int)(value.X - _rectangle.Width / 2.0);
            // _rectangle.Y = (int)(value.Y - _rectangle.Height / 2.0);
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

    public void Initialize() {
    }

    public void Update(GameTime gameTime) {
    }

    public virtual bool OnCollision(ICollisionShape other) {
        Console.WriteLine("Also bad bad bad");
        // throw new NotImplementedException();
        if (!_isStatic)
            return true;
        // Static objects should not resolve collisions
        // Non-static objects have to handle collision with a static object themselves
        return false;
    }
    
}
