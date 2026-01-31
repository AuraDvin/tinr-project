using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics;
using ProjectTINR.Classes.Physics.Shapes;

namespace ProjectTINR.Classes.Objects;

public abstract class Projectile : GameObject, IPhysicsObject {
    public virtual float Scale { get; set; } = 1f;
    public Projectile(Game game) : base(game) {
        if (!FacingRight) {
            Velocity = new Vector2(-Velocity.X, 0);
        }
    }
    public virtual CollisionShapeType CollisionType { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 Position { get; set; }
    public bool FacingRight { get; set; }
}
