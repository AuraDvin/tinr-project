using System;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.ObjectsComponents;

public class ControllerFactory {
    public static IController CreateController(Game game, ControllerType type) {
        return type switch {
            ControllerType.PlayerController => new PlayerController(game),
            _ => throw new ArgumentOutOfRangeException(nameof(type), $"Unsupported controller type: {type}"),
        };
    }
}
