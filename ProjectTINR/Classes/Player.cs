using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes;

public class Player(Game game) : GameObject(game), MoveableComp {

    protected PlayerState _playerState = PlayerState.None;
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

            if (Velocity.X != 0) {
                return PlayerState.Moving;
            }

            return PlayerState.Idling;
        }
    }
    PlayerDirection _playerDirection;
    public PlayerDirection Direction {
        get => _playerDirection;
    }

    private readonly PlayerController _playerController = new();
    private Vector2 _position = new(0, 0);
    private Vector2 _velocity = new(0, 0);
    public Vector2 Velocity {
        get => _velocity;
        set {
            if (value.LengthSquared() > 250000.0f) {
                value.Normalize();
                value *= 500.0f;
            }
            else if (value.LengthSquared() < 0.01f) {
                value = Vector2.Zero;
            }
            _velocity = value;
        }
    }
    public Vector2 Position { get => _position; set => _position = value; }

    public override void Update(GameTime gameTime) {
        _playerController.Update(gameTime);
        UpdatePhysics(gameTime);
    }

    protected void UpdatePhysics(GameTime gameTime) {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        float accel = 200f;
        float friction = 8f;

        if (_playerController.IsMovingLeft) {
            _velocity.X -= accel * dt;
            _playerDirection = PlayerDirection.Left;
            _playerState = PlayerState.Moving;
        }

        if (_playerController.IsMovingRight) {
            _velocity.X += accel * dt;
            _playerDirection = PlayerDirection.Right;
            _playerState = PlayerState.Moving;
        }

        if (!_playerController.IsMovingLeft && !_playerController.IsMovingRight) {
            _velocity = Vector2.Lerp(_velocity, Vector2.Zero, friction * dt);
            _playerState = PlayerState.Idling;
            // _playerDirection = PlayerDirection.Right;
        }

        Velocity = _velocity;
        _position += Velocity * dt;
    }
}
