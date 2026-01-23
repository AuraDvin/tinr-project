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
    public float ShootingDelay { get; set; } = 0.4f;
    public bool JustJumped => _justJumped;
    public bool IsMovingLeft => _isMovingLeft;
    public bool IsMovingRight => _isMovingRight;
    public override void Initialize() {
    }
    public override void Update(GameTime gameTime) {
        if (Scene == null) throw new Exception("[PlayerController -> Scene Manipulator] Scene was not initalized!");
        if (Owner == null) {
            Owner = Scene.FindByType<Player>() ?? throw new Exception("Player class not found in Scene!");
        }
        // Player player = Scene.FindByType<Player>() ?? throw new Exception("Player class not found in Scene!");
        _lastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_lastShot >= ShootingDelay) {
            _canShoot = true;
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
            // Console.WriteLine("Player started jumping.");
        }
        else {
            // Jump on the key release
            if (ks.IsKeyUp(_jump) && _isJumping) {
                _isJumping = false;
                // Console.WriteLine("Player released jump.");
                _justJumped = true;
            }
            else {
                // Console.WriteLine("Player is not jumping.");
                _justJumped = false;
            }
            _isMovingRight = ks.IsKeyDown(_moveRight);
            _isMovingLeft = ks.IsKeyDown(_moveLeft);
        }

        Player player = Owner as Player ?? throw new Exception("Player class not found in Scene!");

        // Todo: add throwing knife to scene, and give it inital position facing the right way
        if (ks.IsKeyDown(_shoot)) {
            // Console.WriteLine("X is down");
            if (_canShoot) {
                // Console.WriteLine("Player can shoot");
                _justShot = true;
                _canShoot = false;
                _lastShot = 0f;
                // int dir = player.Direction == PlayerDirection.Right ? 1 : -1;
                Vector2 playerPos = player.Position;
                PlayerProjectile projectile = new(Game) {
                    Position = playerPos,
                    FacingRight = player.Direction == PlayerDirection.Right,
                };
                Scene.Add(projectile);
                // PlayerProjectileCollisionShape thing = new(playerPos, dir, Game) {
                //     Scene = Scene
                // };
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

        // TODO: think, can we have wall sliding and possibly falling sfx play if the player state is not proper. 
        // and on top of that how will we know if the other things are also true at the same time? 
        // Should the player have multiple states?
        if (JustJumped) {
            player.State = PlayerState.Jumping;
        }
        else {
            if (!player.OnFloor && !isMoving) {
                player.State = PlayerState.Falling;
            }

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

