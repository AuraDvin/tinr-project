using System;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.NPCs;

public class FlyingEnemy : StationaryEnemy {
    public FlyingEnemy(Game game) : base(game) {
    }
    public override CollisionShapeType CollisionType { get; set; } = CollisionShapeType.Rectangle;
}
