using System.IO;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.Physics;

public interface ICollisionShape : IPositionComponent {
    bool ShouldSimulate { get { return false; } }
}
