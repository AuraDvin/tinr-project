using System;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.NPCs;

public class StationaryEnemy(Game game) : Enemy(game) {
    public override ControllerType ControllerType { get => ControllerType.AiController; }
    public override CollisionShapeType CollisionType { get; set; } = CollisionShapeType.EnemyCollisionShape;
    public override Vector2 Velocity { get => _velocity; set { _velocity = value; _velocity.X = 0; } }

    private Vector2 _velocity;

}
