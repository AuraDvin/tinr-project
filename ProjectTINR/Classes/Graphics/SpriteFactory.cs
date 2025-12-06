using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR.Classes.Objects;

using TINR.Classes;

namespace ProjectTINR.Classes.Graphics;

public class SpriteFactory {
    public static Sprite CreateSprite(Game game, GameObject gameObject) {
        AnimatedSprite animatedSprite;
        Sprite sprite;
        if (gameObject is Player) {
            Console.WriteLine("Creating player sprite");
            animatedSprite = new AnimatedSprite(game, Vector2.Zero, game.Content.Load<Texture2D>("images/characters"));
            animatedSprite.AddAnimationFromJson("Content/Spritesheet_edited.json");
            animatedSprite.PlayAnimation("idle");
            return animatedSprite;
        }

        if (gameObject is Floor) {
            Console.WriteLine("Creating floor sprite");
            sprite = new Sprite(game, new(0, 0, 1000, 400), new(0, 0), game.Content.Load<Texture2D>("images/test"));
            return sprite;
        }
        // if object is enemy ... 
        sprite = new Sprite(game, new(0, 0, 194, 194), new(0, 0), game.Content.Load<Texture2D>("images/characters"));
        return sprite;
    }
}
