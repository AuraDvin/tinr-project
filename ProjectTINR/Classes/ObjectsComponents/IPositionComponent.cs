using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.ObjectsComponents;

public interface IPositionComponent : IGameComponent {
    public Vector2 Position { get; set; }
}