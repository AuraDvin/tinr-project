using ProjectTINR.Classes.Physics.Shapes;
namespace ProjectTINR.Classes.Physics;

public class CollisionShapeFactory {
    public static ICollisionShape MakeShape(CollisionShapeType type) {
        return type switch {
            CollisionShapeType.FloorCollisionShape => new FloorCollisionShape(),
            CollisionShapeType.PlayerShape => new PlayerCollisionShape(),
            CollisionShapeType.Rectangle => new RectCollisionShape(false),
            CollisionShapeType.StaticRectangle => new RectCollisionShape(true),
            CollisionShapeType.Circle => new CircleCollisionShape(false, 10.0f),
            CollisionShapeType.StaticCircle => new CircleCollisionShape(true, 10.0f),
            _ => null
        };
    }
}
