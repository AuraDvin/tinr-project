using ProjectTINR.Classes.ObjectsComponents;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace ProjectTINR.Classes;

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
        Player player = Scene.FindByType<Player>() ?? throw new Exception("Player class not found in Scene!");

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
            Console.WriteLine("Player started jumping.");
        }
        else {
            // Jump on the key release
            if (ks.IsKeyUp(_jump) && _isJumping) {
                _isJumping = false;
                Console.WriteLine("Player released jump.");
                _justJumped = true;
            }
            else {
                Console.WriteLine("Player is not jumping.");
                _justJumped = false;
            }
            _isMovingRight = ks.IsKeyDown(_moveRight);
            _isMovingLeft = ks.IsKeyDown(_moveLeft);
        }

        // Todo: add throwing knife to scene, and give it inital position facing the right way
        if (ks.IsKeyDown(_shoot)) {
            if (_canShoot) {
                _justShot = true;
                _canShoot = false;
                _lastShot = 0f;
                float dir = player.Direction == PlayerDirection.Right ? 1f : -1f;
                Vector2 playerPos = player.Position;
                // Make the throwing knife and send it off in the direction from the player's position
                // Scene.Add();
            }
        }

        UpdatePlayerState(player);
    }

    public void UpdatePlayerState(Player player) {
        if (IsMovingLeft) {
            player.Direction = PlayerDirection.Left;
            player.State = PlayerState.Moving;
        }

        if (IsMovingRight) {
            player.Direction = PlayerDirection.Right;
            player.State = PlayerState.Moving;
        }

        if (!IsMovingLeft && !IsMovingRight) {
            player.State = PlayerState.Idling;
        }

        if (JustJumped) {
            player.State = PlayerState.Jumping;
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
}

