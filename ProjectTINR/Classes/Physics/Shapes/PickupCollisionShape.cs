namespace ProjectTINR.Classes.Physics.Shapes;

public class PickupCollisionShape : CircleCollisionShape {
    public PickupCollisionShape() : base(false, 10.0f) {
    }
    public new bool OnCollision(ICollisionShape other) {
        // Tell Scene to remove this node 
        // Tell gameplay to use effect of pickup or change health
        if (other is PlayerCollisionShape) {
            // Pickup collected by player
        }
        return false;
    }
}
