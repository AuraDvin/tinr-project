using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.Objects;

public class PlayerProjectile(Game game) : Projectile(game){
    public override CollisionShapeType CollisionType { get => CollisionShapeType.PlayerProjectile; }
}