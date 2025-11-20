using System.Diagnostics.Contracts;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes;

public interface PositionComp : IGameComponent {
    public Vector2 Position { get; set; }
}