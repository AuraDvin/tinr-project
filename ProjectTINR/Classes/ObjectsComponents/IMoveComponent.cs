using ProjectTINR.Classes.ObjectsComponents;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes;

public interface IMoveComponent : IPositionComponent, IUpdatableGameComponent {
    public Vector2 Velocity { get; set; }
}
