using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR;
using ProjectTINR.Classes;
using ProjectTINR.Classes.Graphics;
using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;

namespace TINR.Classes.Graphics;

public class GameRenderer2D : DrawableGameComponent {
    readonly Level _level;
    readonly SpriteBatch _spriteBatch;
    private readonly Dictionary<string, Sprite> _sprites;
    public GameRenderer2D(Game game, Level level) : base(game) {
        _level = level;
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _sprites = [];
    }
    public override void Update(GameTime gameTime) {
        HashSet<string> updatedObjects = new();
        foreach (GameObject obj in _level.Scene) {
            Sprite sprite;
            if (obj is not IDrawableGameComponent) {
                continue;
            }

            if (!_sprites.ContainsKey(obj.Name)) {
                sprite = SpriteFactory.CreateSprite(Game, obj);
                Game.Components.Add(sprite);
                _sprites.Add(obj.Name, sprite);
            }
            else {
                sprite = _sprites[obj.Name];
            }

            updatedObjects.Add(obj.Name);

            if (obj is not IUpdatableGameComponent) {
                continue;
            }

            if (obj is Player player) {
                // TODO: Generalize this so any sprite can update it's animation
                AnimatedSprite playerSprite = (AnimatedSprite)sprite;
                playerSprite.PlayAnimation(
                    player.State switch {
                        PlayerState.Idling => "idle",
                        PlayerState.Moving => "walk",
                        PlayerState.Jumping => "jump",
                        PlayerState.Falling => "idle",
                        _ => throw new NotImplementedException()
                    });
                playerSprite.SpriteEffects = player.Direction switch {
                    PlayerDirection.Left => SpriteEffects.FlipHorizontally,
                    PlayerDirection.Right => SpriteEffects.None,
                    _ => throw new NotImplementedException()
                };
                _sprites[player.Name] = playerSprite;
            }
        }

        // deload unused objects
        HashSet<string> deleteMe = [];
        foreach (string key in _sprites.Keys) {
            if (!updatedObjects.Contains(key)) {
                deleteMe.Add(key);
            }
        }

        foreach (string key in deleteMe) {
            Game.Components.Remove(_sprites[key]);
            _sprites.Remove(key);
        }

        base.Update(gameTime);
    }

    protected override void LoadContent() {
        base.LoadContent();
    }

    public override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap);
        foreach (GameObject obj in _level.Scene) {
            if (!_sprites.ContainsKey(obj.Name)) {
                continue;
            }

            Sprite sprite = _sprites[obj.Name];
            if (obj is IPositionComponent pos) {
                _spriteBatch.Draw(
                    sprite.Texture,
                    pos.Position,
                    sprite.Rect,
                    Color.White,
                    /* Rotation */ 0f,
                    /* Origin */ Vector2.Zero,
                    /* Scale */ Vector2.One,
                    sprite.SpriteEffects,
                    /* LayerDepth */ 0f
                );
            }
        }
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}