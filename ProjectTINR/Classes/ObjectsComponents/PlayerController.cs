using ProjectTINR.Classes.ObjectsComponents;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace ProjectTINR.Classes;

public class PlayerController(Game game): GameObject(game), IController {

    private Keys _moveLeft = Keys.Left, _moveRight = Keys.Right, _jump = Keys.Space;

    protected bool _isMovingLeft = false;
    protected bool _isMovingRight = false;
    protected bool _isJumping = false;
    protected bool _justJumped = false;
    public bool JustJumped {
        get { return _justJumped; }
    }
    public bool IsMovingLeft {
        get { return _isMovingLeft; }
    }
    public bool IsMovingRight {
        get { return _isMovingRight; }
    }
    public override void Initialize() {
    }
    public override void Update(GameTime gameTime) {
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
            } else {
                Console.WriteLine("Player is not jumping.");
                _justJumped = false;
            }
            _isMovingRight = ks.IsKeyDown(_moveRight);
            _isMovingLeft = ks.IsKeyDown(_moveLeft);
        }
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
}
