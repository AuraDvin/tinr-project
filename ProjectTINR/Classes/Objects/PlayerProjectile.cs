using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.Objects;

public class PlayerProjectile : Projectile{
    public PlayerProjectile(Game game) : base(game)
    {
    }
    public override CollisionShapeType CollisionType { get => CollisionShapeType.PlayerProjectile; }
}