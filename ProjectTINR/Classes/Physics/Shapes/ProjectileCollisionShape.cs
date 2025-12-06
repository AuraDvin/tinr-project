using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Physics.Shapes;

public class ProjectileCollisionShape : CircleCollisionShape, ISceneManipulator {
    private readonly float _lifeTime = 4f;
    private float _sinceBorn = 0f;
    private readonly Projectile _owner;
    bool _deleted = false;
    public ProjectileCollisionShape(Vector2 startingPosition, int direction, Game game) : base(false, 40f) {
        Console.WriteLine("making projectile");
        _position = startingPosition;
        if (direction == 0) direction = 1;
        // give projectile an initial horizontal speed
        const float initialSpeed = 600f;
        _velocity = new Vector2(initialSpeed * Math.Sign(direction), 0f);
        _owner = new(this, game) {
            Position = _position,
            Velocity = _velocity
        };
    }

    public override bool OnCollision(ICollisionShape other) {
        Console.WriteLine("projectile collided");
        if (!_deleted) {
            _deleted = true;
            Scene.Remove(_owner);
        }
        return false;
    }

    public override void Update(GameTime gameTime) {
        Console.WriteLine("Projectile update called");
        if (_deleted) return;
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _position += _velocity * dt;
        _owner.Position = _position;
        _sinceBorn += dt;
        if (_sinceBorn >= _lifeTime) {
            Console.WriteLine("Projectile ready to be removed");
            _deleted = true;
            Scene.Remove(_owner);
            return;
        }
        base.Update(gameTime);
    }
    protected Scene _scene;
    public virtual Scene Scene { 
        get => _scene; 
        set { 
            if (ReferenceEquals(_scene, value)) return; // already set to this scene
            _scene = value;
            if (_scene != null && !_scene.Contains(_owner)) {
                _scene.Add(_owner);
                Console.WriteLine("Projectile owner added to scene"); 
            }
        } 
    }
}
