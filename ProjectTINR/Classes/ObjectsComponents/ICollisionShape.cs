using System;

using ProjectTINR.Classes.Physics;
using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.ObjectsComponents;

public interface ICollisionShape : IPositionComponent {
    public bool ShouldSimulate { get { return false; } }
    virtual bool OnCollision(ICollisionShape other) {
        Console.WriteLine("Bad bad bad bad.");
        return false;
    }
    virtual void BeginFrame() { }
    IStaticPhysicsObject Owner { get; set; }
    protected Vector2 Offset { get; set; }
}
