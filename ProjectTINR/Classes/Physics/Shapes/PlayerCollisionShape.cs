using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Physics.Shapes;

public class PlayerCollisionShape : RectCollisionShape, ISceneManipulator {
    protected float _playerAccel = 200f;
    protected float _playerJumpForce = 100000f;
    protected float _playerGravity = 10000f;
    protected float _playerFriction = 8f;
    public override Vector2 Offset { get => new(50, 0); }
    public override bool OnFloor { get; set; } = false;
    protected float _msSinceLastJump = 0f;
    protected readonly float _jumpTimerBeforeApplyingGravity = 0.3333333334f/2;

    public override void Update(GameTime gameTime) {
        Player player = Owner as Player ?? throw new Exception("Where is the player reference?");
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 objVeloc = Velocity;
        switch (player.State) {
            case PlayerState.Idling:
                objVeloc = Vector2.Lerp(objVeloc, new(0, objVeloc.Y), _playerFriction * dt);
                break;
            case PlayerState.Moving:
                if (player.Direction == PlayerDirection.Left) {
                    if (objVeloc.X > 0f && WasOnFloor) {
                        objVeloc.X = 0f;
                    }
                    objVeloc.X += -_playerAccel * dt;
                }
                else if (player.Direction == PlayerDirection.Right) {
                    if (objVeloc.X < 0f && WasOnFloor) {
                        objVeloc.X = 0f;
                    }
                    objVeloc.X += _playerAccel * dt;
                }
                break;
            case PlayerState.Jumping:
                if (WasOnFloor) {
                    Console.WriteLine("Player is jumping from floor.");
                    Console.WriteLine($"Velocity before: {objVeloc}");
                    objVeloc.Y = -_playerJumpForce;
                    objVeloc.X = player.Direction == PlayerDirection.Left ? -_playerJumpForce : _playerJumpForce;
                    Console.WriteLine($"Velocity after: {objVeloc}");
                    _msSinceLastJump = 0f;
                }
                else {
                    Console.WriteLine("Player is in the air, cannot jump again.");
                }
                break;
            case PlayerState.Falling:
                ;
                break;
            case PlayerState.Frozen:
                objVeloc = Vector2.Zero;
                break;
            default:
                break;
        }

        if (WasOnFloor) {
            objVeloc.Y = Math.Min(objVeloc.Y, 0);
        }

        _msSinceLastJump += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_msSinceLastJump >= _jumpTimerBeforeApplyingGravity) {
            // Apply gravity 
            objVeloc.Y += _playerGravity * dt;
        }

        Velocity = objVeloc;
        // Console.WriteLine($"Ok so I'm Player with this velocity now: {Velocity}");
        // Console.WriteLine($"Ok so I'm Player with this Position now: {Position}");
    }

    public PlayerCollisionShape() : base(false) {
        _rectangle = new Rectangle(0, 0, 194 / 2, 194);
    }
    public override bool OnCollision(ICollisionShape other) {
        // Console.WriteLine("PlayerCollisionShape OnCollision called.");
        if (other is FloorCollisionShape floor) {
            // Console.WriteLine($"PlayerCollisionShape OnCollision with Floor.");
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
                // Console.WriteLine("Player On floor");
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

        return true;
    }
    public Scene Scene { get; set; } = null;
}
