using System.Collections.Generic;
using System.Reflection.Metadata;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR;
using ProjectTINR.Classes;
using ProjectTINR.Classes.ObjectsComponents;

namespace TINR.Classes.Graphics;

public class GameRenderer : DrawableGameComponent {
    readonly Level _level;
    readonly SpriteBatch _spriteBatch;
    private readonly ContentManager _content;
    protected Texture2D _characters;

    private readonly AnimatedSprite _playerSprite;
    private SpriteEffects _playerSpriteEffect;

    public GameRenderer(Game game, Level level) : base(game) {
        _level = level;
        _content = game.Content;
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);

        _playerSprite = new AnimatedSprite(game, new Vector2(100, 100), game.Content.Load<Texture2D>("images/characters"));
        _playerSprite.AddAnimationFromJson("Content/Spritesheet_edited.json");
        _playerSprite.PlayAnimation("idle");

        game.Components.Add(_playerSprite);
    }

    public override void Update(GameTime gameTime) {
        foreach (GameObject obj in _level.Scene) {
            if (obj is not IUpdatableGameComponent) {
                continue;
            }

            if (obj is Player player) {
                _playerSprite.PlayAnimation(
                    player.State switch {
                        PlayerState.Idling => "idle",
                        PlayerState.Moving => "walk",
                        PlayerState.Jumping => "jump",
                        PlayerState.Falling => "idle",
                        _ => throw new System.NotImplementedException()
                    });
                _playerSpriteEffect = player.Direction switch {
                    PlayerDirection.Left => SpriteEffects.FlipHorizontally,
                    PlayerDirection.Right =>SpriteEffects.None, 
                    _ => throw new System.NotImplementedException()
                };

            }
        }
        base.Update(gameTime);
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
                _spriteBatch.Draw(
                    _characters, 
                    player.Position, 
                    _playerSprite.Rect, 
                    Color.White,
                    0f,
                    Vector2.Zero,
                    Vector2.One,
                    _playerSpriteEffect,
                    0f
                );

                continue;
            }
        }
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}