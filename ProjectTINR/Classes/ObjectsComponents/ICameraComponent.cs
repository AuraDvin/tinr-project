using System;

namespace ProjectTINR.Classes.ObjectsComponents;

public interface ICameraComponent : IPositionComponent {
    public float Zoom { get; set; }
}
