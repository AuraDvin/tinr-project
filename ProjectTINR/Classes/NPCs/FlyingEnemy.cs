using System;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.NPCs;

public class FlyingEnemy(Game game) : StationaryEnemy(game) {
    public override CollisionShapeType CollisionType { get; set; } = CollisionShapeType.Rectangle;
}
