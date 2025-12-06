using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes;

public class Player(Game game) : GameObject(game), IPhysicsObject, IDrawableGameComponent {
    protected override string _prefix => "Player";
    public Vector2 Position { get => _position; set => _position = value; }
    public CollisionShapeType CollisionType { get => CollisionShapeType.PlayerShape; set { } }

    public PlayerState State {
        // We could have a timed status (Like frozen) so we should return that just in case
        set => _playerState = value;
        get {
            if (_playerState != PlayerState.None) {
                return _playerState;
            }

            if (Velocity.Y > 0) {
                return PlayerState.Falling;
            }

            if (Velocity.Y < 0) {
                return PlayerState.Jumping;
            }

            return PlayerState.Idling;
        }
    }

    public PlayerDirection Direction { get; private set; }

    public Vector2 Velocity {
        get => _velocity;
        set {
            // if (value.LengthSquared() > 250000.0f) {
            //     value.Normalize();
            //     value *= 500.0f;
            // }
            // else if (value.LengthSquared() <= 1f) {
            //     value = Vector2.Zero;
            // }
            _velocity = value;
        }
    }


    public override void Initialize() {
        base.Initialize();
        _playerController.Initialize();
        // _position = new Vector2(0, -1000);
        // _position = new Vector2(0, 0);
    }

    public override void Update(GameTime gameTime) {
        _playerController.Update(gameTime);
        UpdateMovement(gameTime);
    }

    protected void UpdateMovement(GameTime gameTime) {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // float accel = 200f;
        // float friction = 8f;

        if (_playerController.IsMovingLeft) {
            // _velocity.X -= accel * dt;
            Direction = PlayerDirection.Left;
            _playerState = PlayerState.Moving;
        }

        if (_playerController.IsMovingRight) {
            // _velocity.X += accel * dt;
            Direction = PlayerDirection.Right;
            _playerState = PlayerState.Moving;
        }

        if (!_playerController.IsMovingLeft && !_playerController.IsMovingRight) {
            // _velocity = Vector2.Lerp(_velocity, Vector2.Zero, friction * dt);
            _playerState = PlayerState.Idling;
            // _playerDirection = PlayerDirection.Right;
        }

        if (_playerController.JustJumped) {
            _playerState = PlayerState.Jumping;
            // _velocity.Y = -60000f;
        }

        // Velocity = _velocity;
        _position += Velocity * dt;
        Console.WriteLine("Player velocity: " + _velocity.ToString());
        Console.WriteLine("Player Position: " + _position.ToString());
    }

    protected PlayerState _playerState = PlayerState.None;
    private readonly PlayerController _playerController = new();
    private Vector2 _position = new(0, 0);
    private Vector2 _velocity = new(0, 0);
}
