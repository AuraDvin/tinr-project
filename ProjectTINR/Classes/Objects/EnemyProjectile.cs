using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.Objects;

public class EnemyProjectile(Game game) : Projectile(game) {
    public override CollisionShapeType CollisionType { get; set; } = CollisionShapeType.EnemyProjectile;
}