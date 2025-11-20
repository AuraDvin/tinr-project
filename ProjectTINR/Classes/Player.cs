using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR.Classes.ObjectsComponents;

using TINR.Classes;

namespace ProjectTINR.Classes;

public class Player : GameObject, IUpdatableGameComponent, IDrawableGameComponent, MoveableComp {
    private readonly AnimatedSprite _animatedSprite;
    private readonly PlayerController _playerController;
    private Vector2 _position;
    private Vector2 _velocity;

    public Vector2 Velocity { get => _velocity; set => _velocity = value; }
    public Vector2 Position { get => _position; set => _position = value; }

    public Player(Game game)
    // : base(game)
    {

        _playerController = new PlayerController();
        _animatedSprite = new AnimatedSprite(new Vector2(100, 100), game.Content.Load<Texture2D>("chracters"));

        _animatedSprite.AddAnimationFromJson("Content/Spritesheet_edited.json");
        _animatedSprite.PlayAnimation("idle");

    }

    public void Update(GameTime gameTime) {
        _playerController.Update(gameTime); 


        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        float accel = 400f;
        float friction = 6f;

        if (_playerController.IsMovingLeft) {
            _velocity.X -= accel * dt;
            Console.WriteLine("Moving Left");
        }

        if (_playerController.IsMovingRight) {
            _velocity.X += accel * dt;
            Console.WriteLine("Moving Right");
        }
        if (_velocity.Length() > 500.0f) {
            _velocity.Normalize();
            _velocity *= 500.0f;
        }

        if (!_playerController.IsMovingLeft && !_playerController.IsMovingRight) {
            _velocity = Vector2.Lerp(_velocity, Vector2.Zero, friction * dt);
        }

        _position += _velocity * dt;

        _animatedSprite.Position = _position;   
        _animatedSprite.Update(gameTime);
    }

    public void Draw(SpriteBatch sp) {
        _animatedSprite.Draw(sp);
        // sp.Draw()
        // throw new NotImplementedException("This shouldn't be called (for now), Player - draw"); 
    }

}
