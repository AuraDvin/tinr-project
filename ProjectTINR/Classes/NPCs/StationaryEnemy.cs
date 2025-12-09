using System;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.NPCs;

public class StationaryEnemy : Enemy {
    public StationaryEnemy(Game game) : base(game) {
    }
    public override ControllerType ControllerType { get => ControllerType.AiController; }
    public override CollisionShapeType CollisionType { get; set; } = CollisionShapeType.EnemyCollisionShape;

}
