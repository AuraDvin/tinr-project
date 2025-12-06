using System;

namespace ProjectTINR.Classes.ObjectsComponents;

public class ControllerFactory {
    public static IController CreateController(ControllerType type) {
        return type switch {
            ControllerType.PlayerController => new PlayerController(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), $"Unsupported controller type: {type}"),
        };
    }
}
