using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Physics.Shapes;

public class PlayerProjectileCollisionShape : ProjectileCollisionShape, ISceneManipulator {
    public PlayerProjectileCollisionShape(Vector2 startingPosition, int direction) : base(startingPosition, direction) {
    }
}
