using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.ObjectsComponents;

public interface IMoveComponent : IPositionComponent, IUpdatableGameComponent {
    public Vector2 Velocity { get; set; }
}
