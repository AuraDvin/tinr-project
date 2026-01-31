using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Objects;

public class EnemyProjectile(Game game) : Projectile(game), ISoundPlayer, IDrawableGameComponent {
    public override CollisionShapeType CollisionType { get; set; } = CollisionShapeType.EnemyProjectile;
}