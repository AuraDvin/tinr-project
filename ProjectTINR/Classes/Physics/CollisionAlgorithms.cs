using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.NPCs;
using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics.Shapes;

namespace ProjectTINR.Classes.Physics;

public class CollisionAlgorithms {
    public static bool CheckCollision(ICollisionShape shapeA, ICollisionShape shapeB) {
        // Console.WriteLine($"Between collisions! {shapeA.GetType()}, {shapeB.GetType()}");
        switch (shapeA) {
            // Ignore projectiles and their owners 
            // Ignore projctiles between themselves 
            case PlayerCollisionShape when shapeB is PlayerProjectileCollisionShape:
            case PlayerProjectileCollisionShape when shapeB is PlayerProjectileCollisionShape:
            case PlayerProjectileCollisionShape when shapeB is PlayerCollisionShape:
            case EnemyProjectileCollisionShape when shapeB is EnemyCollisionShape:
            case EnemyCollisionShape when shapeB is EnemyProjectileCollisionShape:
            case ProjectileCollisionShape when shapeB is ProjectileCollisionShape:
                return false;
            // Collide player with enemies
            case PlayerCollisionShape when shapeB is EnemyCollisionShape:
            case EnemyCollisionShape when shapeB is PlayerCollisionShape:
            case FlyingEnemyCollisionShape when shapeB is PlayerCollisionShape:
            case PlayerCollisionShape when shapeB is FlyingEnemyCollisionShape:
                return CheckCollision(shapeA as RectCollisionShape, shapeB as RectCollisionShape);
            // Fallbacks 
            case CircleCollisionShape ca when shapeB is CircleCollisionShape cb:
                return CheckCollision(ca, cb);
            case RectCollisionShape ra when shapeB is RectCollisionShape rb:
                return CheckCollision(ra, rb);
            case CircleCollisionShape ca when shapeB is RectCollisionShape rb:
                return CheckCollision(rb, ca);
            case RectCollisionShape ra when shapeB is CircleCollisionShape cb:
                return CheckCollision(ra, cb);
            default:
                throw new NotImplementedException();
        }
    }
    public static bool CheckCollision(RectCollisionShape rect, CircleCollisionShape circle) {
        // No need to check collision between two static objects
        if (!rect.ShouldSimulate && !circle.ShouldSimulate) return false;

        Rectangle r = rect.Rectangle;
        Vector2 c = circle.Position;

        // Find the closest point on the rectangle to the circle center
        float closestX = Math.Clamp(c.X, r.Left, r.Right);
        float closestY = Math.Clamp(c.Y, r.Top, r.Bottom);

        float dx = c.X - closestX;
        float dy = c.Y - closestY;
        float distSq = dx * dx + dy * dy;
        float radius = circle.Radius;

        if (distSq > radius * radius) {
            return false;
        }
        return true; 
        
    }

    public static bool CheckCollision(CircleCollisionShape circleA, CircleCollisionShape circleB) {
        if (!circleA.ShouldSimulate && !circleB.ShouldSimulate) return false;
        float minDistance = circleA.Radius + circleB.Radius;
        Vector2 vector = circleA.Position - circleB.Position;

        return vector.LengthSquared() <= minDistance * minDistance;
    }

    public static bool CheckCollision(RectCollisionShape rectA, RectCollisionShape rectB) {
        // No need to check collision between two static objects
        if (!rectA.ShouldSimulate && !rectB.ShouldSimulate) {
            return false;
        }

        Rectangle a = rectA.Rectangle;
        Rectangle b = rectB.Rectangle;

        if (!a.Intersects(b)) return false;
        return true;
    }

    public static void ResolveCollision(RectCollisionShape rectA, RectCollisionShape rectB) {
        if (!rectA.ShouldSimulate || !rectB.ShouldSimulate) {
            throw new NotSupportedException("Cannot resolve collision with static physics objects!");
        }
        // Simple collision resolution by swapping velocities (placeholder)
        Vector2 tempVelocityA = rectA.Velocity;
        Vector2 tempVelocityB = rectB.Velocity;
        rectA.Velocity = tempVelocityB;
        rectB.Velocity = tempVelocityA;
    }

    public static void ResolveCollision(ICollisionShape shapeA, ICollisionShape shapeB) {
        if (!shapeA.ShouldSimulate || !shapeB.ShouldSimulate) {
            throw new NotSupportedException("Cannot resolve collision with static physics objects!");
        }
    }
    public static void ResolveCollision(CircleCollisionShape circle, RectCollisionShape rect) {
        
        Rectangle r = rect.Rectangle;
        Vector2 c = circle.Position;
        float radius = circle.Radius;
        
        // Find the closest point on the rectangle to the circle center
        float closestX = Math.Clamp(c.X, r.Left, r.Right);
        float closestY = Math.Clamp(c.Y, r.Top, r.Bottom);

        float dx = c.X - closestX;
        float dy = c.Y - closestY;
        float distSq = dx * dx + dy * dy;

        float distance = (float)Math.Sqrt(Math.Max(distSq, 0f));
        Vector2 mtv;

        if (distance == 0f) {
            // Circle center is exactly on/inside rectangle; push out along shortest axis
            // Determine penetration to each side and push out along minimal one
            float leftPen = c.X - r.Left;
            float rightPen = r.Right - c.X;
            float topPen = c.Y - r.Top;
            float bottomPen = r.Bottom - c.Y;

            // Find minimal penetration
            float minPen = Math.Min(Math.Min(leftPen, rightPen), Math.Min(topPen, bottomPen));
            if (minPen == leftPen) mtv = new Vector2(-1, 0);
            else if (minPen == rightPen) mtv = new Vector2(1, 0);
            else if (minPen == topPen) mtv = new Vector2(0, -1);
            else mtv = new Vector2(0, 1);
            // penetration amount: radius + minPen
            float penetration = radius + minPen;
            mtv *= penetration;
        }
        else {
            float penetration = radius - distance;
            mtv = new Vector2(dx / distance, dy / distance) * penetration;
        }

        // Apply translation splitting based on ShouldSimulate flags
        bool moveRect = rect.ShouldSimulate;
        bool moveCircle = circle.ShouldSimulate;

        if (moveRect && moveCircle) {
            // split movement half-half (rect moves opposite of circle)
            Vector2 moveCircleBy = mtv * 0.5f;
            Vector2 moveRectBy = -mtv * 0.5f;

            circle.Position = circle.Position + moveCircleBy;
            // shift rectangle by integer amounts
            r.X += (int)Math.Round(moveRectBy.X);
            r.Y += (int)Math.Round(moveRectBy.Y);
            rect.Rectangle = r;
        }
        else if (moveCircle) {
            circle.Position = circle.Position + mtv;
        }
        else if (moveRect) {
            r.X += (int)Math.Round(-mtv.X);
            r.Y += (int)Math.Round(-mtv.Y);
            rect.Rectangle = r;
        }
    }
}
