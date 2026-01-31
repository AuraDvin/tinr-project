using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics;
using ProjectTINR.Classes.Physics.Shapes;

namespace ProjectTINR.Classes.Objects;

public class PickupObject : GameObject, ISceneManipulator, IDrawableGameComponent, IPhysicsObject {
    public virtual PickupType Type { get; set; }
    public PickupObject(Game game) : base(game) {
    }

    public Scene Scene { get; set; }
    public Vector2 Position { get; set; }
    public CollisionShapeType CollisionType {
        get => CollisionShapeType.Pickup;
        set { }
    }
    public Vector2 Velocity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public virtual void Collected() {
        // Notify player of collected effect
        if (Scene != null) {
            Scene.Remove(this);
        }
        else {
            throw new Exception("didn't have scene reference");
        }
    }
}
