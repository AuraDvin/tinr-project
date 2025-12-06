using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Physics.Shapes;

public class PlayerProjectileCollisionShape(Vector2 startingPosition, int direction, Game game)
    : ProjectileCollisionShape(startingPosition, direction, game), ISceneManipulator {
}
