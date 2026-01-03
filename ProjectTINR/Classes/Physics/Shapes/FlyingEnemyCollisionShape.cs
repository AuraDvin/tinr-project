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
            // If we hit the top of a floor, bounce upward a bit
            if (_rectangle.Bottom > floor.Rectangle.Top) {
                Velocity = new(Velocity.X, -Math.Abs(Velocity.Y));
            }
            return false;
        }
        return false;
    }
}
