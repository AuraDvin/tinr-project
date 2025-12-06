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
        set => _velocity = value;
    }


    public override void Initialize() {
        _playerController.Initialize();
        base.Initialize();
    }

    public override void Update(GameTime gameTime) {
        _playerController.Update(gameTime);
        UpdateMovement(gameTime);
    }

    protected void UpdateMovement(GameTime gameTime) {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_playerController.IsMovingLeft) {
            Direction = PlayerDirection.Left;
            _playerState = PlayerState.Moving;
        }

        if (_playerController.IsMovingRight) {
            Direction = PlayerDirection.Right;
            _playerState = PlayerState.Moving;
        }

        if (!_playerController.IsMovingLeft && !_playerController.IsMovingRight) {
            _playerState = PlayerState.Idling;
        }

        if (_playerController.JustJumped) {
            _playerState = PlayerState.Jumping;
        }

        _position += Velocity * dt;
        // Console.WriteLine("Player velocity: " + _velocity.ToString());
        // Console.WriteLine("Player Position: " + _position.ToString());
    }

    protected PlayerState _playerState = PlayerState.None;
    private readonly PlayerController _playerController = new();
    private Vector2 _position = new(0, 0);
    private Vector2 _velocity = new(0, 0);
}
