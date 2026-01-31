using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Objects;

public class PlayerProjectile(Game game) : Projectile(game), ISoundPlayer{
    public float Scale = 1f;
    public override CollisionShapeType CollisionType { get => CollisionShapeType.PlayerProjectile; }
}