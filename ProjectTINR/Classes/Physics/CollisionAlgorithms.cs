using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.Physics.Shapes;

namespace ProjectTINR.Classes.Physics;

struct RectF {
    public float X, Y, Width, Height;

    public float Left => X;
    public float Right => X + Width;
    public float Top => Y;
    public float Bottom => Y + Height;

    public Point Center => new Point((int)(X + Width / 2), (int)(Y + Height / 2));

    public static RectF FromRectangle(Rectangle rect) =>
        new RectF { X = rect.X, Y = rect.Y, Width = rect.Width, Height = rect.Height };
    public Rectangle ToRectangle() =>
        new Rectangle((int)X, (int)Y, (int)Width, (int)Height);

    public bool Intersects(RectF other) {
        return !(other.Left >= Right ||
                    other.Right <= Left ||
                    other.Top >= Bottom ||
                    other.Bottom <= Top);
    }
}

public class CollisionAlgorithms {
    public static bool CheckCollision(ICollisionShape shapeA, ICollisionShape shapeB) {
        // if (shapeA is PlayerCollisionShape playerShape && shapeB is FloorCollisionShape floorShape) {
        //     return CheckCollision(playerShape, floorShape);
        // }
        if (shapeA is PlayerCollisionShape && shapeB is PlayerProjectileCollisionShape ||
                shapeA is PlayerProjectileCollisionShape && shapeB is PlayerCollisionShape) {
            return false;
        }

        if (shapeA is RectCollisionShape rectA && shapeB is RectCollisionShape rectB) {
            return CheckCollision(rectA, rectB);
        }
        // Add other shape combinations here as needed
        return false;
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
        // Simple collision resolution by swapping velocities (placeholder)
        Vector2 tempVelocityA = rectA.Velocity;
        Vector2 tempVelocityB = rectB.Velocity;
        rectA.Velocity = tempVelocityB;
        rectB.Velocity = tempVelocityA;
    }

    public static void ResolveCollision(ICollisionShape shapeA, ICollisionShape shapeB) {
        // Implement collision resolution logic for generic shapes if needed
    }
}
