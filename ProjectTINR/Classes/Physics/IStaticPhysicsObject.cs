using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Physics;

public interface IStaticPhysicsObject : IPositionComponent {
   public CollisionShapeType CollisionType { get; set; }
}
