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
    protected float _playerJumpForce = 600f;
    protected float _playerGravity = 1000f;
    protected float _playerSlideGravity = 600f;
    protected float _playerFriction = 8f;
    public override Vector2 Offset { get => new(50, 0); }
    private int _wallJumpCount = 0;
    private readonly int _maxWallJumps = 1;
    protected readonly float _jumpTimerBeforeApplyingGravity = 0.005f;
    protected bool _collectedPickup = false;
    protected float _msSinceLastJump;
    protected bool _tookDmg = false;
    protected float _lastTookDmg = 0f;
    protected float _immuneFramesMS = 33f;

    public PlayerCollisionShape() : base(false) {
        // Apply gravity right away 
        _msSinceLastJump = _jumpTimerBeforeApplyingGravity;
        Rectangle = new Rectangle(0, 0, 194 / 2, 194);
    }

    public override void Update(GameTime gameTime) {
        Player player = Owner as Player ?? throw new Exception("Where is the player reference?");
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 objVeloc = Velocity;
        bool shouldApplyGravity = false;
        _msSinceLastJump += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Apply gravity if enough time has passed
        if (_msSinceLastJump >= _jumpTimerBeforeApplyingGravity) {
            shouldApplyGravity = true;
        }

        switch (player.State) {
            case PlayerState.Idling:
                objVeloc = Vector2.Lerp(objVeloc, new(0, objVeloc.Y), _playerFriction * dt);
                break;
            case PlayerState.Shooting:
                break;
            case PlayerState.Sliding:
                if (shouldApplyGravity) {
                    objVeloc.Y += _playerSlideGravity * dt;
                    shouldApplyGravity = false;
                }
                goto case PlayerState.Moving;
            case PlayerState.Moving:
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
                break;
            case PlayerState.Jumping:
                if (WasOnFloor) {
                    objVeloc.Y = -_playerJumpForce;
                    _msSinceLastJump = 0f;
                }
                else if (WasOnWall && _wallJumpCount < _maxWallJumps) {
                    objVeloc.Y = -_playerJumpForce;
                    if (player.Direction == PlayerDirection.Left) {
                        objVeloc.X = _playerJumpForce / 2;
                    }
                    else if (player.Direction == PlayerDirection.Right) {
                        objVeloc.X = -_playerJumpForce / 2;
                    }
                    _wallJumpCount++;
                }
                break;
            case PlayerState.Falling:
                if (shouldApplyGravity) {
                    objVeloc.Y += _playerGravity * dt;
                    shouldApplyGravity = false;
                }
                break;

            case PlayerState.Frozen:
                objVeloc = Vector2.Zero;
                break;
            default:
                break;
        }


        if (WasOnFloor) {
            objVeloc.Y = Math.Min(objVeloc.Y, 0);
            _wallJumpCount = 0;
        }

        if (shouldApplyGravity) {
            objVeloc.Y += _playerGravity * dt;
        }

        Velocity = objVeloc;

        // This should be in PlayerController (?) I get it's a oncollision type event but feels out of place
        if (_tookDmg) {
            if (_lastTookDmg <= 0f) {
                _lastTookDmg = _immuneFramesMS / 1000f;
                (Owner as Player).TakeDamage();
                _tookDmg = false;
            }
            else {
                _lastTookDmg -= dt;
            }
        }
    }

    public override void BeginFrame() {
        base.BeginFrame();
        _collectedPickup = false;
    }
    public override bool OnCollision(ICollisionShape other) {
        // snaps to floor, returns ShouldSimulate which is always true for player
        // so the type needs to be checked as well

        // TODO
        // if other is PickupCollisionShape 
        // give pickup type to player?

        if (other is FloorCollisionShape) {
            base.OnCollision(other);
            (Owner as Player).OnFloor = OnFloor;
            (Owner as Player).OnWall = OnWall;
            return false;
        }

        if (other is EnemyProjectileCollisionShape) {
            _tookDmg = true;
            return false;
        }

        return true;
    }
    public Scene Scene { get; set; } = null;
}
