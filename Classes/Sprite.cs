using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TINR.Classes;

public class Sprite : DrawableGameComponent {
    private Rectangle _rect;
    private Vector2 _position;
    static Texture2D _texture;

    public Sprite(Game game, Rectangle rect, Vector2 position, Texture2D texture) : base(game) {
        _rect = rect;
        _position = position;
        _texture = texture;
    }

    public void SetRect(Rectangle rect) {
        _rect = rect;
    }

    public override void Initialize() {
        base.Initialize();
    }

    public void Draw(SpriteBatch sp) {
        sp.Draw(_texture,
            _position,
            _rect,
            Color.White);
    }
}