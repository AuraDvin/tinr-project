using Microsoft.Xna.Framework;

using ProjectTINR.Classes.Objects;

namespace ProjectTINR.Classes.Physics.Shapes;

public class EnemyProjectileCollisionShape: ProjectileCollisionShape{
    public EnemyProjectileCollisionShape(Vector2 startingPosition, int direction, Game game) : base(startingPosition, direction, game)
    {
    }
}
