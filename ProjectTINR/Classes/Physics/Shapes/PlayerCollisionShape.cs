using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.NPCs;
using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Physics.Shapes;

public class PlayerCollisionShape : RectCollisionShape, ISceneManipulator {
    protected float _playerAccel = 200f;
    protected float _playerJumpForce = 100f;
    // protected float _playerGravity = 10000f;
    protected float _playerGravity = 1000f;
    protected float _playerFriction = 8f;
    public override Vector2 Offset { get => new(50, 0); }
    public override bool OnFloor { get; set; } = false;
    protected float _msSinceLastJump = 0f;
    protected readonly float _jumpTimerBeforeApplyingGravity = 2;

    protected bool _tookDmg = false;
    protected float _lastTookDmg = 0f;
    protected float _immuneFramesMS = 33f;

    public override void Update(GameTime gameTime) {
        Player player = Owner as Player ?? throw new Exception("Where is the player reference?");
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 objVeloc = Velocity;
        switch (player.State) {
            case PlayerState.Idling:
                objVeloc = Vector2.Lerp(objVeloc, new(0, objVeloc.Y), _playerFriction * dt);
                break;
            case PlayerState.Moving:
                // Console.WriteLine($"[PlayerCollisionShape] Player direction: {player.Direction}");
                int sign;
                switch (player.Direction) {
                    case PlayerDirection.Left:
                        if (objVeloc.X > 0f) {
                            objVeloc.X = 0f;
                        }
                        sign = -1;
                        break;
                    case PlayerDirection.Right:
                        if (objVeloc.X < 0f) {
                            objVeloc.X = 0f;
                        }
                        sign = 2;
                        break;
                    default:
                        throw new Exception("Invalid player direction!");
                }
                objVeloc.X += sign * _playerAccel * dt;
                Console.WriteLine($"[PlayerCollisionShape] Player state: {player.State} {player.Direction} x: {objVeloc.X} y: {objVeloc.Y}");
                break;
            case PlayerState.Jumping:
                if (WasOnFloor) {
                    // Console.WriteLine("Player is jumping from floor.");
                    // Console.WriteLine($"Velocity before: {objVeloc}");
                    objVeloc.Y = -_playerJumpForce;
                    // objVeloc.X = player.Direction == PlayerDirection.Left ? -_playerJumpForce : _playerJumpForce;
                    // Console.WriteLine($"Velocity after: {objVeloc}");
                    _msSinceLastJump = 0f;
                }
                else {
                    // Console.WriteLine("Player is in the air, cannot jump again.");
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

        if (_tookDmg) {
            if (_lastTookDmg <= 0f) {
                _lastTookDmg = _immuneFramesMS / 1000f;
                (Owner as Player).takeDamage();
                _tookDmg = false;
            }
            else {
                _lastTookDmg -= dt;
            }
        }

        // Console.WriteLine($"Ok so I'm Player with this velocity now: {Velocity}");
        // Console.WriteLine($"Ok so I'm Player with this Position now: {Position}");
    }

    public PlayerCollisionShape() : base(false) {
        _rectangle = new Rectangle(0, 0, 194 / 2, 194);
    }
    public override bool OnCollision(ICollisionShape other) {

        if (other is EnemyProjectileCollisionShape) {
            _tookDmg = true;
        }

        if (other is FloorCollisionShape floor) {
            Rectangle floorRect = floor.Rectangle;

            // overlap (at least one) should always be non zero here
            float overlapX = Math.Min(floorRect.Right, _rectangle.Right) - Math.Max(floorRect.Left, _rectangle.Left);
            float overlapY = Math.Min(floorRect.Bottom, _rectangle.Bottom) - Math.Max(floorRect.Top, _rectangle.Top);

            if (overlapX < overlapY) {
                if (_rectangle.Center.X < floorRect.Center.X) {
                    // Player to the left of the floor (wall)
                    Console.WriteLine("Player is to the left of the floor!");
                    Velocity = new(Math.Min(Velocity.X, 0), Velocity.Y);
                    Owner.Position = new Vector2(floorRect.Left - _rectangle.Width - Offset.X + 1, Owner.Position.Y);
                }
                else {
                    // Player to the right of the floor (wall)
                    Console.WriteLine("Player is to the right of the floor!");
                    Velocity = new(Math.Max(Velocity.X, 0), Velocity.Y);
                    Owner.Position = new Vector2(floorRect.Right + Offset.X - 1, Owner.Position.Y);
                }
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

   
        return true;
    }
    public Scene Scene { get; set; } = null;
}
