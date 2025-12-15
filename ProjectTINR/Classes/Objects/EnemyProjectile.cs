using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.Objects;

public class EnemyProjectile : Projectile {
    public EnemyProjectile(Game game) : base(game)
    {
    }

    public override CollisionShapeType CollisionType { get; set; } = CollisionShapeType.EnemyProjectile;
}