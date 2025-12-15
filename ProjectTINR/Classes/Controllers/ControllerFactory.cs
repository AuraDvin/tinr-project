using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.NPCs;
using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Controllers;

public class ControllerFactory {
    public static IController CreateController(Game game, IControlled owner) {
        ControllerType type = owner.ControllerType;
        return type switch {
            ControllerType.PlayerController => new PlayerController(game){Owner = owner as Player},
            ControllerType.AiController => new EnemyController(game){Owner = owner as StationaryEnemy},
            ControllerType.FlyingAiController => new EnemyController(game){Owner = owner as FlyingEnemy},
            _ => throw new ArgumentOutOfRangeException(nameof(type), $"Unsupported controller type: {type}"),
        };
    }
}
