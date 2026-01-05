using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR.Classes.NPCs;
using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Graphics;

public class SpriteFactory {
    private static readonly string CharactersPath = "images/characters";
    private static readonly string PlayerAnimationPath = "Content/Spritesheet_edited.json";
    private static readonly string TestFloorPath = "images/test";
    private static readonly Rectangle FloorTextureRect = new(0, 0, 1000, 400);
    private static readonly Rectangle StationaryEnemyTextureRect = new(0, 0, 194, 194);
    
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

        if (gameObject is Floor) {
            // Console.WriteLine("Creating floor sprite");
            sprite = new Sprite(game, FloorTextureRect, Vector2.Zero, game.Content.Load<Texture2D>(TestFloorPath));
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
        return null;
    }
}
