using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes;

public class Player(Game game) : GameObject(game), IPhysicsObject, IDrawableGameComponent, IControlled {
    protected override string _prefix => "Player";
    public Vector2 Position { get => _position; set => _position = value; }
    public CollisionShapeType CollisionType { get => CollisionShapeType.PlayerShape; set { } }
    public ControllerType ControllerType => ControllerType.PlayerController;
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
    public PlayerDirection Direction { get => _direction; set => _direction = value; }
    public Vector2 Velocity {
        get => _velocity;
        set => _velocity = value;
    }

    public override void Initialize() {
        base.Initialize();
    }

    public override void Update(GameTime gameTime) {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _position += Velocity * dt;
        // Console.WriteLine("Player velocity: " + _velocity.ToString());
        // Console.WriteLine("Player Position: " + _position.ToString());
        base.Update(gameTime);
    }
    private PlayerDirection _direction = PlayerDirection.Right;
    protected PlayerState _playerState = PlayerState.None;
    private Vector2 _position = new(0, 0);
    private Vector2 _velocity = new(0, 0);
}
