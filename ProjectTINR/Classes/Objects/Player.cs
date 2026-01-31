using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.Levels;
using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics;

namespace ProjectTINR.Classes.Objects;

public class Player(Game game) : GameObject(game), IPhysicsObject, IDrawableGameComponent, IControlled, ISoundPlayer {
    protected override string _prefix => "Player";
    public Vector2 LastCheckpoint = new();
    public Vector2 Position {
        get => _position;
        set => _position = value;
    }
    readonly float PICKUP_TIMEOUT = 5f;
    public float ShootDelayFactor { get; private set; } = 1f;
    public float ProjectileSizeFactor { get; private set; } = 1f;
    public bool OnFloor { get; set; } = false;
    public bool OnWall { get; set; } = false;
    public CollisionShapeType CollisionType { get => CollisionShapeType.PlayerShape; set { } }
    public ControllerType ControllerType => ControllerType.PlayerController;
    public PlayerState State {
        // We could have a timed status (Like frozen) so we should return that just in case
        set => _playerState = value;
        get { return _playerState; }
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
        _lastDamageTimer += dt;
        List<PickupType> timersToRemove = [];
        foreach (var (keys, val) in _pickuptimers) {
            if (val >= PICKUP_TIMEOUT) {
                timersToRemove.Add(keys);
                switch (keys) {
                    case PickupType.SHOOT_SPEED:
                        resetProjectileSpeed();
                        break;
                    case PickupType.BIGGER_PROJECTILE:
                        resetProjectileSize();
                        break;
                }
            }
            _pickuptimers[keys] += dt;
        }

        foreach (var thing in timersToRemove) {
            _pickuptimers.Remove(thing);
        }

        if (Position.Y >= 1000) _health = 0;

        base.Update(gameTime);
    }
    private PlayerDirection _direction = PlayerDirection.Right;
    protected PlayerState _playerState = PlayerState.None;
    private Vector2 _position = new(0.0f, 0.0f);
    private Vector2 _velocity = new(0, 0);
    protected int _health = 3;
    public int Health => _health;
    private readonly float _immuneFramesS = 1f;
    private float _lastDamageTimer = 0f;
    public void TakeDamage() {
        if (_lastDamageTimer < _immuneFramesS) return;
        _health--;
        _lastDamageTimer = 0f;
    }

    public void HealDamage() {
        _health++;
    }

    public Player(int initHealth, Game game) : this(game) {
        _health = initHealth;
    }

    public void IncreaseProjectileSize() {
        ProjectileSizeFactor = 2f;
        _pickuptimers.Add(PickupType.BIGGER_PROJECTILE, 0f);
    }

    private void resetProjectileSize() {
        ProjectileSizeFactor = 1f;
    }

    private void resetProjectileSpeed() {
        ShootDelayFactor = 1f;
    }

    public void IncreaseProjectileSpeed() {
        ShootDelayFactor = 0.5f;
        _pickuptimers.Add(PickupType.SHOOT_SPEED, 0f);
    }
    public void CollectCheckpoint(Checkpoint checkpoint) {
        LastCheckpoint = checkpoint.Position;
    }

    private readonly Dictionary<PickupType, float> _pickuptimers = [];
}
