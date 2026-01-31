using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics;

namespace ProjectTINR.Classes.Objects;

public class Player(Game game) : GameObject(game), IPhysicsObject, IDrawableGameComponent, IControlled, ISoundPlayer {
    protected override string _prefix => "Player";
    public Vector2 Position {
        get => _position;
        set => _position = value;
    }
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
        
        base.Update(gameTime);
    }
    private PlayerDirection _direction = PlayerDirection.Right;
    protected PlayerState _playerState = PlayerState.None;
    private Vector2 _position = new(0.0f, 0.0f);
    private Vector2 _velocity = new(0, 0);
    protected int _health = 3;
    public int Health => _health;
    
    public void TakeDamage() {
        _health--;
    }
    
    public void HealDamage() {
        _health++;
    }

    public Player(int initHealth, Game game) : this(game){
        _health = initHealth;
    }
}
