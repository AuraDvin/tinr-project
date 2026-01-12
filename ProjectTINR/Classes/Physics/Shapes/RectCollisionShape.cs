using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Physics.Shapes;

public class RectCollisionShape(bool isStatic) : ICollisionShape, IMoveComponent {
    public virtual bool OnFloor { get; set; } = false;
    public virtual bool OnWall { get; set; } = false;
    protected Rectangle _rectangle = new(0, 0, 128, 128);
    // protected Vector2 _velocity;
    private readonly bool _isStatic = isStatic;
    public bool ShouldSimulate { get => !_isStatic; }
    public virtual Vector2 Position {
        get => Owner.Position + Offset;
        set {
        }
    }

    public Rectangle Rectangle {
        get {
            return new((int)Position.X, (int)Position.Y, _rectangle.Width, _rectangle.Height);
        }
        set {
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
        // Snap floor collision
        if (other is FloorCollisionShape floor) {
            Rectangle floorRect = floor.Rectangle;

            int myRightSide = (int)(Position.X + _rectangle.Width);
            int myBottomSide = (int)(Position.Y + _rectangle.Height);
            // overlap (at least one) should always be non zero here
            float overlapX = Math.Min(floorRect.Right, myRightSide) - Math.Max(floorRect.Left, Position.X);
            float overlapY = Math.Min(floorRect.Bottom, myBottomSide) - Math.Max(floorRect.Top, Position.Y);
            
            if (overlapX < overlapY) {
                OnWall = true;
                if (_rectangle.Center.X < floorRect.Center.X) {
                    // Wall is on the right
                    Velocity = new(Math.Min(Velocity.X, 0), Velocity.Y);
                    Owner.Position = new Vector2(floorRect.Left - _rectangle.Width - Offset.X + 1, Owner.Position.Y);
                    
                }
                else {
                    // Wall is on the left
                    Velocity = new(Math.Max(Velocity.X, 0), Velocity.Y);
                    Owner.Position = new Vector2(floorRect.Right - Offset.X - 1, Owner.Position.Y);
                }
                // Console.WriteLine($"Snapping player to wall x: {Owner.Position.X}, overlaps {new Vector2(overlapX, overlapY)}, floor: {floorRect} obj: { _rectangle} ");
            }
            else {
                // Player on top of floor
                if (_rectangle.Center.Y < floorRect.Center.Y) {
                    Velocity = new(Velocity.X, 0);
                    Owner.Position = new Vector2(Owner.Position.X, floorRect.Top - _rectangle.Height - Offset.Y + 1);
                    OnFloor = true;
                }
                // Player under the floor
                else {
                    Velocity = new(Velocity.X, Math.Max(Velocity.Y, 0));
                    Owner.Position = new Vector2(Owner.Position.X, floorRect.Bottom + Offset.Y - 1);
                }
            }
            return false;
        }
        return ShouldSimulate;
    }


    public virtual void Update(GameTime gameTime) {
    }

    // Remember previous-frame floor contact, then clear transient state for this frame
    public bool WasOnFloor { get; private set; } = false;
    public bool WasOnWall { get; private set; } = false;

    public virtual void BeginFrame() {
        WasOnFloor = OnFloor;
        WasOnWall = OnWall;
        OnFloor = false;
        OnWall = false;
    }

    public IStaticPhysicsObject Owner { get; set; }
    public virtual Vector2 Offset { get; set; } = Vector2.Zero;
    public virtual void Initialize() {
    }
}
