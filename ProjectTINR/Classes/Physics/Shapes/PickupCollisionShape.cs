using System;
using System.Reflection.Metadata.Ecma335;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Physics.Shapes;

public class PickupCollisionShape : CircleCollisionShape {
    public override Vector2 Offset { get => new(62, 64); set { } }
    public PickupCollisionShape() : base(true, 55f) {
    }
    public override bool OnCollision(ICollisionShape other) {
        if (other is PlayerCollisionShape) {
            (Owner as PickupObject).Collected();
        }
        return false;
    }
}
