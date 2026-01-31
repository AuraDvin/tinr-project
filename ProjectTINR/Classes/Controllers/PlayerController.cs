using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics.Shapes;

namespace ProjectTINR.Classes.Controllers;

public class PlayerController(Game game) : GameObject(game), IController, ISceneManipulator {
    private Keys _moveLeft = Keys.Left;
    private Keys _moveRight = Keys.Right;
    private Keys _jump = Keys.Space;
    private Keys _shoot = Keys.X;
    private float _ogShootdelay = 1f;
    public float ShootingDelay { get; set; } = 1f;
    public bool JustJumped => _justJumped;
    public bool IsMovingLeft => _isMovingLeft;
    public bool JustAttacked => _justShot;
    public bool IsMovingRight => _isMovingRight;
    public override void Initialize() {
    }
    public override void Update(GameTime gameTime) {
        if (Scene == null) throw new Exception("[PlayerController -> Scene Manipulator] Scene was not initalized!");
        if (Owner == null) {
            Owner = Scene.FindByType<Player>() ?? throw new Exception("Player class not found in Scene!");
        }

        ShootingDelay = _ogShootdelay * (Owner as Player).ShootDelayFactor;

        // Player player = Scene.FindByType<Player>() ?? throw new Exception("Player class not found in Scene!");
        _lastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_lastShot >= ShootingDelay) {
            _canShoot = true;
            _justShot = false;
        }
        else {
            if (_justShot) {
                _canShoot = false;
            }
        }

        var ks = Keyboard.GetState();
        // Don't allow left/right movement before jump
        if (ks.IsKeyDown(_jump)) {
            _isJumping = true;
        }
        // Jump on the key release
        else {
            if (ks.IsKeyUp(_jump) && _isJumping) {
                _isJumping = false;
                _justJumped = true;
            }
            else {
                _justJumped = false;
            }
            _isMovingRight = ks.IsKeyDown(_moveRight);
            _isMovingLeft = ks.IsKeyDown(_moveLeft);
        }

        Player player = Owner as Player ?? throw new Exception("Player class not found in Scene!");

        if (ks.IsKeyDown(_shoot)) {
            if (_canShoot) {
                _justShot = true;
                _canShoot = false;
                _lastShot = 0f;
                
                Vector2 playerPos = player.Position;
                PlayerProjectile projectile = new(Game) {
                    Position = playerPos,
                    FacingRight = player.Direction == PlayerDirection.Right,
                    Scale = player.ProjectileSizeFactor,
                };
                Scene.Add(projectile);
            }
        }

        UpdatePlayerState(player);
    }

    void UpdatePlayerState(Player player) {
        bool isMoving = IsMovingLeft || IsMovingRight;
        
        if (isMoving) {
            player.State = PlayerState.Moving;
            player.Direction = IsMovingLeft ? PlayerDirection.Left : PlayerDirection.Right;
        }
        else {
            player.State = PlayerState.Idling;
        }
        // This must be set after above since either or happens
        // The second check is to hopefully make the time you have no controller over the player 
        // as little as possible

        if (JustAttacked && _lastShot < 0.5f) {
            player.State = PlayerState.Shooting;
        }

        if (JustJumped) {
            player.State = PlayerState.Jumping;
        }
        else {
            // Just freefalling
            if (!player.OnFloor && !isMoving) {
                player.State = PlayerState.Falling;
            }
            // does have to handle the movement input
            if (player.OnWall && isMoving) {
                player.State = PlayerState.Sliding;
            }
        }

       
    }

    private float _lastShot = 0f;
    private bool _canShoot = true;
    protected bool _isMovingLeft = false;
    protected bool _isMovingRight = false;
    protected bool _isJumping = false;
    protected bool _justJumped = false;
    protected bool _justShot = false;
    public Scene Scene { get; set; } = null;
    public GameObject Owner { get; set; }
}

