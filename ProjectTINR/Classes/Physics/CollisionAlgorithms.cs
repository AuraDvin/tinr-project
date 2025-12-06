using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.Physics.Shapes;

namespace ProjectTINR.Classes.Physics;

public class CollisionAlgorithms {
    public static bool CheckCollision(ICollisionShape shapeA, ICollisionShape shapeB) {
        switch (shapeA) {
            case PlayerCollisionShape when shapeB is PlayerProjectileCollisionShape:
            case PlayerProjectileCollisionShape when shapeB is PlayerProjectileCollisionShape:
            case PlayerProjectileCollisionShape when shapeB is PlayerCollisionShape:
                return false;
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

        // Collision detected. Compute minimal translation vector (MTV)
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

        // Compute penetration along each axis
        int overlapX = Math.Min(a.Right, b.Right) - Math.Max(a.Left, b.Left);
        int overlapY = Math.Min(a.Bottom, b.Bottom) - Math.Max(a.Top, b.Top);

        // Choose the axis with the smallest overlap (closest faces)
        bool separateHorizontal = overlapX <= overlapY;

        // Determine direction signs for A and B along the chosen axis
        int signA;
        if (separateHorizontal) {
            signA = (a.Center.X < b.Center.X) ? -1 : 1; // A should move left if it's left of B
        }
        else {
            signA = (a.Center.Y < b.Center.Y) ? -1 : 1; // A should move up if it's above B
        }
        int signB = -signA;

        // Binary-search (integer) for the minimal shift that separates the rectangles.
        // We consider shifting A by signA * t and B by signB * t; the actual applied
        // shift may be split between objects depending on ShouldSimulate.
        int low = 0;
        int high = Math.Max(Math.Max(a.Width, a.Height), Math.Max(b.Width, b.Height));
        if (high <= 0) high = 1;

        // increase high until separated (cap to avoid infinite loop)
        Rectangle ta, tb;
        int safety = 0;
        while (true) {
            ta = a;
            tb = b;
            int shiftA = signA * high;
            int shiftB = signB * high;
            if (separateHorizontal) {
                if (rectA.ShouldSimulate) ta.X += shiftA;
                if (rectB.ShouldSimulate) tb.X += shiftB;
            }
            else {
                if (rectA.ShouldSimulate) ta.Y += shiftA;
                if (rectB.ShouldSimulate) tb.Y += shiftB;
            }
            if (!ta.Intersects(tb)) break;
            high *= 2;
            safety++;
            if (safety > 30) break; // avoid pathological infinite loops
        }

        // Binary search between low and high
        while (low < high) {
            int mid = (low + high) / 2;
            ta = a;
            tb = b;
            int shiftA = signA * mid;
            int shiftB = signB * mid;
            if (separateHorizontal) {
                if (rectA.ShouldSimulate) ta.X += shiftA;
                if (rectB.ShouldSimulate) tb.X += shiftB;
            }
            else {
                if (rectA.ShouldSimulate) ta.Y += shiftA;
                if (rectB.ShouldSimulate) tb.Y += shiftB;
            }
            if (ta.Intersects(tb)) {
                low = mid + 1;
            }
            else {
                high = mid;
            }
        }

        int minimalShift = low;

        // Apply the minimal shift, splitting it between objects if both simulate
        if (minimalShift > 0) {
            int shiftForA, shiftForB;
            if (rectA.ShouldSimulate && rectB.ShouldSimulate) {
                shiftForA = minimalShift / 2;
                shiftForB = minimalShift - shiftForA;
            }
            else if (rectA.ShouldSimulate) {
                shiftForA = minimalShift;
                shiftForB = 0;
            }
            else if (rectB.ShouldSimulate) {
                shiftForA = 0;
                shiftForB = minimalShift;
            }
            else {
                shiftForA = 0;
                shiftForB = 0;
            }

            if (separateHorizontal) {
                a.X += signA * shiftForA;
                b.X += signB * shiftForB;
            }
            else {
                a.Y += signA * shiftForA;
                b.Y += signB * shiftForB;
            }
        }

        rectA.Rectangle = a;
        rectB.Rectangle = b;
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
}
