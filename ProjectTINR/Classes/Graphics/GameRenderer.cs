using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR.Classes;
using ProjectTINR.Classes.ObjectsComponents;

namespace TINR.Classes.Graphics;

public class GameRenderer : DrawableGameComponent {
    Level _level;
    SpriteBatch _spriteBatch;
    private ContentManager _content;
    protected Texture2D _characters;

    private AnimatedSprite _playerSprite;

    public GameRenderer(Game game, Level level) : base(game) {
        _level = level;
        _content = game.Content;
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _playerSprite = new AnimatedSprite(game, new Vector2(100, 100), game.Content.Load<Texture2D>("images/characters"));
        _playerSprite.AddAnimationFromJson("Content/Spritesheet_edited.json");
        _playerSprite.PlayAnimation("idle");


        game.Components.Add(_playerSprite);

    }

    protected override void LoadContent() {
        _characters = _content.Load<Texture2D>("images/characters");
        base.LoadContent();
    }

    public override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        foreach (GameObject obj in _level.Scene) {
            if (obj is not PositionComp) {
                continue;
            }

            if (obj is Player player) {
                _spriteBatch.Draw(_characters, player.Position, _playerSprite.Rect, Color.White);
                continue;
            }
        }
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}