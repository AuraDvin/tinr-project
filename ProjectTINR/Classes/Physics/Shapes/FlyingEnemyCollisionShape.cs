using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Physics.Shapes;

public class FlyingEnemyCollisionShape : RectCollisionShape {
    public FlyingEnemyCollisionShape() : base(false) {
    }

    public override void Update(GameTime gameTime) {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 v = Velocity;
        // light air dampingdwas dwdw as sawds
        v *= (float)Math.Pow(0.98, dt * 60f);
        Velocity = v;
        base.Update(gameTime);
    }

    public override bool OnCollision(ICollisionShape other) {
        if (other is FloorCollisionShape floor) {
            int myRightSide = (int)(Position.X + _rectangle.Width);
            int myBottomSide = (int)(Position.Y + _rectangle.Height);
            float overlapX = Math.Min(floor.Rectangle.Right, myRightSide) - Math.Max(floor.Rectangle.Left, Position.X);
            float overlapY = Math.Min(floor.Rectangle.Bottom, myBottomSide) - Math.Max(floor.Rectangle.Top, Position.Y);

            Vector2 velocityBefore = Velocity;
            base.OnCollision(other); // snap to floor 
            Vector2 velocityAfter = Velocity;

            // bounce off the floors, walls, ceilings
            if (overlapX < overlapY) {
                // wall
                Velocity = new(-velocityBefore.X * 0.7f, velocityAfter.Y);
            }
            else {
                // floor or ceiling
                Velocity = new(velocityAfter.X, -velocityBefore.Y * 0.7f);
            }
            // Floor is static so collision won't be resolved by the physics engine anyway 
            return false;
        }
        return false;
    }
}
