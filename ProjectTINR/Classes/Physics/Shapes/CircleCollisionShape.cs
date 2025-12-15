using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;

using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProjectTINR.Classes.Physics.Shapes;

public class CircleCollisionShape(bool isStatic, float radius) : ICollisionShape, IMoveComponent {
    protected float _radius = radius;
    public bool ShouldSimulate { get => !isStatic; }

    public Vector2 Position {
        get => Owner.Position + Offset;
        set => Owner.Position = value - Offset;
    }

    public Vector2 Velocity {
        get {
            if (!ShouldSimulate) return Vector2.Zero;
            IPhysicsObject obj = Owner as IPhysicsObject ?? throw new Exception("Not a physics object");
            return obj.Velocity;
        }
        set {
            if (!ShouldSimulate) throw new  Exception("Updating Velocity of a static object");
            IPhysicsObject obj = Owner as IPhysicsObject ?? throw new Exception("Not a physics object");
            obj.Velocity = value;
        }
    }
    public float Radius { get => _radius; set => _radius = value; }

    public virtual void Initialize() {
    }
    public virtual void Update(GameTime gameTime) {
    }
    public virtual bool OnCollision(ICollisionShape other) {
        if (ShouldSimulate)
            return true;
        // Static objects should not resolve collisions
        // Non-static objects have to handle collision with a static object themselves
        return false;
    }

    public IStaticPhysicsObject Owner { get; set; }
    public Vector2 Offset { get; set; } =  Vector2.Zero;
}
