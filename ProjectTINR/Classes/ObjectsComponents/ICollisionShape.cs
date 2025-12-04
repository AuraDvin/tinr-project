using System.IO;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.Physics;

public interface ICollisionShape : IPositionComponent {
    public bool ShouldSimulate { get { return false; } }
    public bool OnCollision(ICollisionShape other);
}
