using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR;
using ProjectTINR.Classes;
using ProjectTINR.Classes.Graphics;
using ProjectTINR.Classes.ObjectsComponents;

namespace TINR.Classes.Graphics;

public class GameRenderer2D : DrawableGameComponent {
    readonly Game _game_ref;
    readonly Level _level;
    readonly SpriteBatch _spriteBatch;
    private readonly ContentManager _content;
    protected Texture2D _characters;
    // private readonly AnimatedSprite _playerSprite;
    private readonly Dictionary<string, Sprite> _sprites;

    public GameRenderer2D(Game game, Level level) : base(game) {
        _game_ref = game;
        _level = level;
        _content = game.Content;
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _sprites = new();

        // _playerSprite = new AnimatedSprite(game, new Vector2(100, 100), game.Content.Load<Texture2D>("images/characters"));
        // _playerSprite.AddAnimationFromJson("Content/Spritesheet_edited.json");
        // _playerSprite.PlayAnimation("idle");

        // game.Components.Add(_playerSprite);
    }

    public override void Update(GameTime gameTime) {
        HashSet<string> updatedObjects = new();
        foreach (GameObject obj in _level.Scene) {
            if (obj is not IDrawableGameComponent) {
                continue;
            }
            if (!_sprites.ContainsKey(obj.Name)) {
                // Add a new sprite variable, has to come from the class 
                // So we should use a SpriteFactory of some sort
                // Specifically for each enemy the 
                Sprite sprite = SpriteFactory.CreateSprite(_game_ref, obj);
                _game_ref.Components.Add(sprite);
                _sprites.Add(obj.Name, sprite);
            }

            if (obj is not IUpdatableGameComponent) {
                continue;
            }

            if (obj is Player player) {
                // TODO: Generalize this so any sprite can update it's animation
                AnimatedSprite _playerSprite = (AnimatedSprite)_sprites[player.Name];
                _playerSprite.PlayAnimation(
                    player.State switch {
                        PlayerState.Idling => "idle",
                        PlayerState.Moving => "walk",
                        PlayerState.Jumping => "jump",
                        PlayerState.Falling => "idle",
                        _ => throw new System.NotImplementedException()
                    });
                _playerSprite.SpriteEffects = player.Direction switch {
                    PlayerDirection.Left => SpriteEffects.FlipHorizontally,
                    PlayerDirection.Right => SpriteEffects.None,
                    _ => throw new System.NotImplementedException()
                };
                _sprites[player.Name] = _playerSprite;
            }
            updatedObjects.Add(obj.Name);
        }

        // deload unused objects
        HashSet<string> deleteMe = [];

        foreach (string key in _sprites.Keys) {
            if (!updatedObjects.Contains(key)) {
                deleteMe.Add(key);
                _game_ref.Components.Remove(_sprites[key]);
            }
        }

        foreach (string name in deleteMe) {
            _sprites.Remove(name);
        }

        base.Update(gameTime);
    }

    protected override void LoadContent() {
        _characters = _content.Load<Texture2D>("images/characters");
        base.LoadContent();
    }

    public override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin( SpriteSortMode.Deferred, BlendState.AlphaBlend,SamplerState.LinearWrap);
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