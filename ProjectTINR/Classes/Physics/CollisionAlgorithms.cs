using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.Physics.Shapes;

namespace ProjectTINR.Classes.Physics;

public class CollisionAlgorithms {
    public static bool CheckCollision(ICollisionShape shapeA, ICollisionShape shapeB) {
        // Implement collision detection logic based on shape types
        // For example, if both are circles, use circle-circle collision detection
        // If one is a rectangle and the other is a circle, use rectangle-circle collision detection, etc.
        Console.WriteLine("I am here actually, not in the method you want me to be");
        // Placeholder implementation
        return false;
    }

    public static bool CheckCollision(RectCollisionShape rectA, RectCollisionShape rectB) {
        // No need to check collision between two static objects
        if (!rectA.ShouldSimulate && !rectB.ShouldSimulate) {
            return false;
        }

        Rectangle a = rectA.Rectangle;
        Rectangle b = rectB.Rectangle;

        if (a.Intersects(b)) {
            Console.WriteLine("Collision detected between rectangles.");
            // Calculate direction from each center
            Vector2 centerA = new Vector2(a.Center.X, a.Center.Y);
            Vector2 centerB = new Vector2(b.Center.X, b.Center.Y);
            Vector2 directionAwayA = Vector2.Normalize(centerA - centerB);
            Vector2 directionAwayB = Vector2.Normalize(centerB - centerA);
            
            bool isColliding = true;
            float value = 1.0f;
            while (value > 0.001f) {
                // Use Bisection to find the point of collision more accurately and set the positions so they are just touching
                if (isColliding) {
                    // Move apart
                    if (rectA.ShouldSimulate) {
                        a.X += (int)(directionAwayA.X * value);
                        a.Y += (int)(directionAwayA.Y * value);
                    }
                    if (rectB.ShouldSimulate) {
                        b.X += (int)(directionAwayB.X * value);
                        b.Y += (int)(directionAwayB.Y * value);
                    }
                }
                else {
                    // Move closer
                    if (rectA.ShouldSimulate) {
                        a.X -= (int)(directionAwayA.X * value);
                        a.Y -= (int)(directionAwayA.Y * value);
                    }
                    if (rectB.ShouldSimulate) {
                        b.X -= (int)(directionAwayB.X * value);
                        b.Y -= (int)(directionAwayB.Y * value);
                    }
                }
                isColliding = a.Intersects(b);
                value *= 0.5f;
            }
            rectA.Rectangle = a;
            rectB.Rectangle = b;
            return true;
        }
        return false;
    }

    public static void ResolveCollision(RectCollisionShape rectA, RectCollisionShape rectB) {
        // Simple collision resolution by reversing velocities
        Vector2 tempVelocityA = rectA.Velocity;
        Vector2 tempVelocityB = rectB.Velocity;
        rectA.Velocity = tempVelocityB;
        rectB.Velocity = tempVelocityA;
    }

    public static void ResolveCollision(ICollisionShape shapeA, ICollisionShape shapeB) {
        // Implement collision resolution logic
        // Adjust positions and velocities of the shapes based on the collision

        // Placeholder implementation
    }

}
