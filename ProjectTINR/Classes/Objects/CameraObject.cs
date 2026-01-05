using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Objects;

public class CameraObject(Game game) : GameObject(game), IControlled, ICameraComponent {
    public ControllerType ControllerType => ControllerType.CameraController;
    public Vector2 Position { get; set; } = Vector2.Zero;
    public float Zoom { get; set; } = 1.0f;
}
