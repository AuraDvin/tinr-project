using System;
using System.Buffers;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Physics.Shapes;

public class PlayerProjectileCollisionShape
    : ProjectileCollisionShape {
    public PlayerProjectileCollisionShape(Vector2 startingPosition, int direction, float scale, Game game) : base(startingPosition, direction, game) {
        // if (Owner is PlayerProjectile pp) {
        //     Console.WriteLine($"Radius {_radius} {Offset} owner: {Owner}");
        // }
        _radius *= scale;
        Offset *= scale;
        Console.WriteLine($"Radius {_radius} {Offset}");
    }
}
