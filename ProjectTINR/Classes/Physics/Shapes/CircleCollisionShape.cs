using System;

using Microsoft.Xna.Framework;

using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProjectTINR.Classes.Physics.Shapes;

public class CircleCollisionShape(bool _isStatic, float radius) : ICollisionShape, IMoveComponent {
    protected float _radius = radius;
    protected Vector2 _position;
    protected Vector2 _velocity;
    public bool ShouldSimulate { get => !_isStatic; }
    public Vector2 Position {get => _position; set => _position = value; }
    public Vector2 Velocity {get => _velocity; set => _velocity = value; }
    public float Radius { get => _radius; set => _radius = value; }

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
