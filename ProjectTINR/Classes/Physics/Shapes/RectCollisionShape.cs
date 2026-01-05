using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Physics.Shapes;

public class RectCollisionShape(bool isStatic) : ICollisionShape, IMoveComponent {
    public virtual bool OnFloor { get; set; } = false;
    protected Rectangle _rectangle = new(0, 0, 128, 128);
    protected Vector2 _velocity;
    private readonly bool _isStatic = isStatic;
    public bool ShouldSimulate { get => !_isStatic; }
    public virtual Vector2 Position {
        get => Owner.Position + Offset;
        set {
            Owner.Position = value;
        }
    }

    public Rectangle Rectangle {
        get {
            return new((int)Position.X, (int)Position.Y, _rectangle.Width, _rectangle.Height);
        }
        set {
            Position = new Vector2(value.X, value.Y) - Offset;
            _rectangle = value;
        }
    }
    public Vector2 Velocity {
        get {
            if (!ShouldSimulate) return Vector2.Zero;
            IPhysicsObject obj = Owner as IPhysicsObject ?? throw new Exception("Not a physics object");
            return obj.Velocity;
        }
        set {
            if (!ShouldSimulate)
                return;
            IPhysicsObject obj = Owner as IPhysicsObject ?? throw new Exception("Not a physics object");
            obj.Velocity = value;
        }
    }

    public virtual bool OnCollision(ICollisionShape other) {
        // Console.WriteLine("Warning! RectCollisionShape collided (missing override?)");
        if (other is FloorCollisionShape floor) {
            Rectangle rect = floor.Rectangle;
            int top = 0, bottom = 1, left = 2, right = 3;
            var distances = new List<float> {
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
            }
            else if (min == bottom) {
                // Console.WriteLine("Player is under the floor!");
                Velocity = new(Velocity.X, Math.Max(Velocity.Y, 0));
            }
            else if (min == left) {
                // Console.WriteLine("Player is to the left of the floor!");
                Velocity = new(Math.Min(Velocity.X, 0), Velocity.Y);
            }
            else if (min == right) {
                // Console.WriteLine("Player is to the right of the floor!");
                Velocity = new(Math.Max(Velocity.X, 0), Velocity.Y);
            }
            return false;
        }
        return ShouldSimulate;
    }


    public virtual void Update(GameTime gameTime) {
    }

    // Remember previous-frame floor contact, then clear transient state for this frame
    public bool WasOnFloor { get; private set; } = false;

    public virtual void BeginFrame() {
        WasOnFloor = OnFloor;
        OnFloor = false;
    }
    public IStaticPhysicsObject Owner { get; set; }
    public virtual Vector2 Offset { get; set; } = Vector2.Zero;
    public virtual void Initialize() {
    }
}
