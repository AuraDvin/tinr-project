using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Graphics;

public class GameRenderer2D(Game game, Level level) : DrawableGameComponent(game) {
    readonly Level _level = level;
    protected readonly SpriteBatch _spriteBatch = new SpriteBatch(game.GraphicsDevice);
    private readonly Dictionary<string, Sprite> _sprites = [];
    private readonly Camera2D _camera = new Camera2D();
    public override void Update(GameTime gameTime) {
        HashSet<string> updatedObjects = new();
        foreach (GameObject obj in _level.Scene) {
            Sprite sprite;

            if (obj is ICameraComponent cam) {
                // Console.WriteLine($"Updating camera from GameRenderer2D. {cam}");
                _camera.Position = cam.Position;
                _camera.Zoom = cam.Zoom;
                // Console.WriteLine($"Camera position: {_camera.Position}, Zoom: {_camera.Zoom}");
            }

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
                        PlayerState.None => "idle",
                        PlayerState.Shooting => "attack",
                        PlayerState.Sliding => "idle",
                        PlayerState.Falling => "idle",
                        PlayerState.Moving => "walk",
                        PlayerState.Jumping => "jump",
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
        Matrix viewMatrix = _camera.GetViewMatrix();
        // _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, transformMatrix: viewMatrix);
        _spriteBatch.Begin(transformMatrix: viewMatrix, samplerState: SamplerState.LinearWrap);
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