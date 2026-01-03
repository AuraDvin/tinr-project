using System;

using ProjectTINR.Classes.NPCs;
using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics.Shapes;
namespace ProjectTINR.Classes.Physics;

public class CollisionShapeFactory {
    public static ICollisionShape MakeShape(IStaticPhysicsObject staticPhysicsObject) {
        Projectile projectile = staticPhysicsObject as Projectile;
        CollisionShapeType type = staticPhysicsObject.CollisionType; 
        return type switch {
            CollisionShapeType.FloorCollisionShape => new FloorCollisionShape(){Owner = staticPhysicsObject},
            CollisionShapeType.PlayerShape => new PlayerCollisionShape(){Owner = staticPhysicsObject as Player},
            CollisionShapeType.Rectangle => new RectCollisionShape(false){Owner = staticPhysicsObject},
            CollisionShapeType.StaticRectangle => new RectCollisionShape(true){Owner = staticPhysicsObject},
            CollisionShapeType.Circle => new CircleCollisionShape(false, 10.0f){Owner = staticPhysicsObject},
            CollisionShapeType.StaticCircle => new CircleCollisionShape(true, 10.0f){Owner = staticPhysicsObject},
            CollisionShapeType.EnemyCollisionShape => new EnemyCollisionShape(){Owner = staticPhysicsObject as StationaryEnemy},
            CollisionShapeType.FlyingEnemyCollisionShape => new FlyingEnemyCollisionShape(){Owner = staticPhysicsObject as FlyingEnemy},
            CollisionShapeType.EnemyProjectile => new EnemyProjectileCollisionShape(
                (projectile ?? throw new Exception("Projectile has no parent")).Position,
                projectile.FacingRight? 1:-1, 
                projectile.Game) {
                Owner = projectile
            },
            CollisionShapeType.PlayerProjectile => new PlayerProjectileCollisionShape(
                (projectile ?? throw new Exception("Projectile has no parent")).Position,
                projectile.FacingRight? 1:-1, 
                projectile.Game) {
                Owner = projectile
            },
            _ => throw new NotImplementedException()
        };
    }
}
