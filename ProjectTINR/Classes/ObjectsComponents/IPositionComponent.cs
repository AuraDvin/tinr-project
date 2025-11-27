using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes;

public interface IPositionComponent : IGameComponent {
    public Vector2 Position { get; set; }
}