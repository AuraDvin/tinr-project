using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics.Shapes;

namespace ProjectTINR.Classes.Objects;

public class Projectile : GameObject, IPhysicsObject {
    public Projectile(ProjectileCollisionShape shape, Game game)  : base(game){
        ProjectileShape = shape;
    }
    public ProjectileCollisionShape ProjectileShape {get; set; }
    public CollisionShapeType CollisionType { get => CollisionShapeType.Projectile; set{} }
    public Vector2 Velocity { get; set; }
    public Vector2 Position { get; set; }    
}
