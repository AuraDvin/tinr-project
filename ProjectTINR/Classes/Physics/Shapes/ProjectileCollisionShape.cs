using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Physics.Shapes;

public abstract class ProjectileCollisionShape : CircleCollisionShape, ISceneManipulator {
    private readonly float _lifeTime = 4f;
    private float _sinceBorn = 0f;
    private const float initialSpeed = 60f;
    bool _deleted = false;

    public override Vector2 Offset { get => new(40, 40); set{ }}
    
    public ProjectileCollisionShape(Vector2 startingPosition, int direction, Game game) : base(false, 40f) {
        // Console.WriteLine("making projectile");
        if (direction == 0) direction = 1;
        // give projectile an initial horizontal speed
        if (Owner != null) {
            Velocity = new Vector2(initialSpeed * direction, 0);
            if (Owner is PlayerProjectile p) {
                _radius *= p.Scale;
            }
        }
    }

    public override bool OnCollision(ICollisionShape other) {
        // Console.WriteLine("projectile collided");
        if (!_deleted) {
            _deleted = true;
            Scene.Remove(Owner);
        }
        return false;
    }

    public override void Update(GameTime gameTime) {
        if (_deleted) return;
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position += Velocity * dt;
        Velocity = new Vector2(initialSpeed * (((Projectile)Owner).FacingRight ? 1 : -1), 0);
        // Console.WriteLine($"Projectile current position {Position}, {Velocity}");
        _sinceBorn += dt;
        if (_sinceBorn >= _lifeTime) {
            // Console.WriteLine("Projectile ready to be removed");
            _deleted = true;
            Scene.Remove(Owner);
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
        } 
    }
}
