using Microsoft.Xna.Framework;

using ProjectTINR.Classes.Objects;

namespace ProjectTINR.Classes.Physics.Shapes;

public class EnemyProjectileCollisionShape(Vector2 startingPosition, int direction, Game game) : ProjectileCollisionShape(startingPosition, direction, game){
}
