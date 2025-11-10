using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TINR.Classes;

namespace TINR;

public class ProjectTinr : Game {
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private readonly LinkedList<Sprite> _sprites;
    private Texture2D _texture;
    private int _animFrame = 0;

    public ProjectTinr() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _sprites = new LinkedList<Sprite>();
    }

    protected override void Initialize() {
        _texture = Content.Load<Texture2D>("chracters");

        _sprites.AddFirst(new AnimatedSprite(this, new Vector2(420.0f, 42.0f), _texture, "Content/Spritesheet_edited.json"));
        _sprites.AddFirst(new AnimatedSprite(this, new Vector2(GraphicsDevice.Viewport.Width * 0.5f, GraphicsDevice.Viewport.Height * 0.5f), _texture, "Content/blobi_animations.json"));
        
       

        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        foreach (AnimatedSprite _sprite in _sprites) {
            _sprite.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        foreach (var sprite in _sprites) {
            sprite.Draw(_spriteBatch);
        }
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}