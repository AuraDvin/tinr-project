using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Physics.Shapes;

public class EnemyCollisionShape : RectCollisionShape {
    public EnemyCollisionShape() : base(false) {
    }

    public override void Update(GameTime gameTime) {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 objVeloc = Velocity;
        if (OnFloor) objVeloc.Y = Math.Min(objVeloc.Y, 0);
        else objVeloc.Y += 10000f * dt;
        Velocity = objVeloc;
        base.Update(gameTime);
    }
    
    public override bool OnCollision(ICollisionShape other) {
        // Console.WriteLine("Enemy colided with something");
        if (other is FloorCollisionShape floor) {
            return base.OnCollision(other);
        }
        return false;
    }
}
