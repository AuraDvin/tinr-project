using System;
using System.IO;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.Physics;

public interface ICollisionShape : IPositionComponent {
    public bool ShouldSimulate { get { return false; } }
    virtual bool OnCollision(ICollisionShape other) {
        Console.WriteLine("Bad bad bad bad.");
        return false;
    }
}
