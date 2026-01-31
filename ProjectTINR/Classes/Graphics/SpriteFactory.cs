using System;
using System.ComponentModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR.Classes.NPCs;
using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.UI;

namespace ProjectTINR.Classes.Graphics;

public class SpriteFactory {
    private static readonly string CharactersPath = "images/characters";
    private static readonly string PlayerAnimationPath = "Content/Spritesheet_edited.json";
    private static readonly string TestFloorPath = "images/test";
    private static readonly Rectangle FloorTextureRect = new(0, 0, 1000, 400);
    private static readonly Rectangle StationaryEnemyTextureRect = new(0, 0, 194, 194);
    private static Texture2D pickupTexture;
    public static Sprite CreateSprite(Game game, GameObject gameObject) {
        AnimatedSprite animatedSprite;
        Sprite sprite;
        if (gameObject is Player) {
            // Console.WriteLine("Creating player sprite");
            animatedSprite = new AnimatedSprite(game, Vector2.Zero, game.Content.Load<Texture2D>(CharactersPath));
            animatedSprite.AddAnimationFromJson(PlayerAnimationPath);
            animatedSprite.PlayAnimation("idle");
            return animatedSprite;
        }

        if (gameObject is Floor floor) {
            // Console.WriteLine("Creating floor sprite");
            sprite = new Sprite(game, floor.BoundingBox, Vector2.Zero, game.Content.Load<Texture2D>(TestFloorPath));
            return sprite;
        }

        if (gameObject is StationaryEnemy) {
            sprite = new Sprite(game, StationaryEnemyTextureRect, Vector2.Zero, game.Content.Load<Texture2D>(CharactersPath));

            return sprite;
        }

        if (gameObject is FlyingEnemy) {
            sprite = new Sprite(game, StationaryEnemyTextureRect, Vector2.Zero, game.Content.Load<Texture2D>(CharactersPath));

            return sprite;
        }

        if (gameObject is PickupObject po) {
            if (pickupTexture == null) {
                pickupTexture = game.Content.Load<Texture2D>("images/pickups");
            }

            Rectangle textureRect;
            switch (po.Type) {
                case PickupType.HEAL:
                    textureRect = new (0, 0, 128, 128);
                    break;
                case PickupType.BIGGER_PROJECTILE:
                    textureRect = new(128, 0, 128, 128);
                    break;
                case PickupType.SHOOT_SPEED:
                    textureRect = new (0, 128, 128, 128);   
                    break;
                case PickupType.CHECKPOINT:
                    textureRect = new (128, 128, 128, 128);
                    break;
                default:
                    throw new Exception("unknown pickup type");
            }
            sprite = new Sprite(game, textureRect, po.Position, pickupTexture);
            return sprite;
        }

        if (gameObject is SimpleUIElement sie) {
            if (sie.HasTexture) {
                sprite = new Sprite(game, sie.TextureRect, Vector2.Zero, game.Content.Load<Texture2D>(sie.TextureName));
                return sprite;
            }
        }
        return null;
    }
}
