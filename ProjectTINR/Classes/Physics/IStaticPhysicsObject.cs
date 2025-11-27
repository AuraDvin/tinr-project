using System;

namespace ProjectTINR.Classes.ObjectsComponents;

public interface IStaticPhysicsObject : IPositionComponent {
   public CollisionShapeType CollisionType { get; set; }
}
