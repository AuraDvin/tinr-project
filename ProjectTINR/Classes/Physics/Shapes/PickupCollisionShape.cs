using System;
using System.Reflection.Metadata.Ecma335;

using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Physics.Shapes;

public class PickupCollisionShape : CircleCollisionShape {
    public PickupCollisionShape() : base(true, 2f) {
    }
    public override bool OnCollision(ICollisionShape other) {
        if (other is PlayerCollisionShape) {
            (Owner as PickupObject).Collected();
        }
        return false;
    }
}
