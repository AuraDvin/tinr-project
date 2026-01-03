using System;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.NPCs;

public class FlyingEnemy(Game game) : Enemy(game) {
    public override ControllerType ControllerType { get => ControllerType.FlyingAiController; }
    public override CollisionShapeType CollisionType { get; set; } = CollisionShapeType.FlyingEnemyCollisionShape;
}
