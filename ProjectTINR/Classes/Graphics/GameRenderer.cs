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

    public GameRenderer(Game game, Level level) : base(game) {
        _level = level;
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        _content = game.Content;
    }

    protected override void LoadContent() {
        _characters = _content.Load<Texture2D>("images/characters");
        base.LoadContent();
    }

    public override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        foreach (GameObject obj in _level.Scene) {
            if (obj is IDrawableGameComponent) {
                // Get the texture 
                // Get the position 
                // Draw with spriteBatch
                // ((Player)obj).Position;
                // ((Player)obj).
                ((IDrawableGameComponent)obj).Draw(_spriteBatch);
                // _spriteBatch.Draw(

                // );
            }
            // Sprite sprite = obj.GetComponent<Sprite>();
            // sprite?.Draw(_spriteBatch);
        }
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}