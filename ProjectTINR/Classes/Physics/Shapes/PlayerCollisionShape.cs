using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.Physics.Shapes;

public class PlayerCollisionShape : RectCollisionShape {
    Vector2 _offset = new(50, 0);
    public bool OnFloor { get; set; } = false;
    public override Vector2 Position {
        get => new Vector2(_rectangle.X, _rectangle.Y) - _offset;
        set {
            _rectangle.X = (int)(value.X + _offset.X);
            _rectangle.Y = (int)(value.Y + _offset.Y);
        }
    }
    public PlayerCollisionShape() : base(false) {
        _rectangle = new Rectangle(0, 0, 194/2, 194);
    }
    public override bool OnCollision(ICollisionShape other) {
        Console.WriteLine("PlayerCollisionShape OnCollision called.");
        if (other is FloorCollisionShape floor) {
            Rectangle rect = floor.Rectangle;
            int top = 0, bottom = 1, left = 2, right = 3;
            var distances =  new List<float> {
                Math.Abs(_rectangle.Bottom - rect.Top), // Top
                Math.Abs(_rectangle.Top - rect.Bottom), // Bottom
                Math.Abs(_rectangle.Right - rect.Left), // Left
                Math.Abs(_rectangle.Left - rect.Right)  // Right
            };
            int min = -1;
            float minDistance = float.MaxValue;
            for (int i = 0; i < distances.Count; i++) {
                if (minDistance > distances[i]) {
                    minDistance = distances[i];
                    min = i;
                }
                
            }

            if (min == top) {
                // Console.WriteLine("Player is on top of the floor!");
                Velocity = new(Velocity.X, Math.Min(Velocity.Y, 0)); 
                OnFloor = true;
            } else if (min == bottom) {
                // Console.WriteLine("Player is under the floor!");
                Velocity = new(Velocity.X, Math.Max(Velocity.Y, 0)); 
            } else if (min == left) {
                // Console.WriteLine("Player is to the left of the floor!");
                Velocity = new(Math.Min(Velocity.X, 0), Velocity.Y);
            } else if (min == right) {
                // Console.WriteLine("Player is to the right of the floor!");
                Velocity = new(Math.Max(Velocity.X, 0), Velocity.Y);
            }
            return false;
        }

        return true;
    }
}
