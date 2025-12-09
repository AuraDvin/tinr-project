using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.Physics.Shapes;

namespace ProjectTINR.Classes.Objects;

public class EnemyProjectile(ProjectileCollisionShape shape, Game game) : Projectile(shape, game) {
}
