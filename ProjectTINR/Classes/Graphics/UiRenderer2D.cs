using System;
using System.Collections.Generic;
using System.Data;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Graphics;

public class UiRenderer2D(Game game, Level level) : GameRenderer2D(game, level) {
    private Level _level = level;
    private Dictionary<string, Sprite> _uiSprites = [];
    
    SpriteFont _spriteFont = game.Content.Load<SpriteFont>("gameFont");

    public override void Draw(GameTime gameTime) {
        Console.WriteLine("Starting draw batch");
        _spriteBatch.Begin();
        foreach (IUiDrawableComponent obj in _level.UIScene) {
            if (!obj.Visible) continue;

            Texture2D texture = obj.Texture;
            string label = obj.String;

            if (label != null) {
                _spriteBatch.DrawString(_spriteFont, label, obj.TextPosition, Color.White);
                Console.WriteLine("Mf string printed");
            }

            if (texture != null) {
                _spriteBatch.Draw(texture, new Rectangle(){X = 0, Y = 0, Width = texture.Width, Height = texture.Height}, Color.White);
                Console.WriteLine("Mf texture drawn");
            }

        }
        _spriteBatch.End();
    }

    public override void Update(GameTime gameTime) {
    }
}
