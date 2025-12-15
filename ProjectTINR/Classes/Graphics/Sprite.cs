using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Graphics;

public class Sprite(Game game, Rectangle rect, Vector2 position, Texture2D texture) : GameComponent(game), IDrawableGameComponent, IPositionComponent {
    protected Rectangle _rect = rect;
    protected Texture2D _texture = texture;
    protected Vector2 _position = position;
    protected SpriteEffects _spriteEffects = SpriteEffects.None;
    public SpriteEffects SpriteEffects { get => _spriteEffects; set => _spriteEffects = value; }
    public Texture2D Texture { get { return _texture; } }
    public Vector2 Position {
        get { return _position; }
        set { _position = value; }
    }

    public void SetRect(Rectangle rect) {
        _rect = rect;
    }

    public Rectangle Rect {
        get { return _rect; }
        set => SetRect(value);
    }

    public override void Initialize() {
        base.Initialize();
    }

    public void Draw(SpriteBatch sp) {
        // Console.WriteLine($"Sprite - Draw at position {_position}");
        sp.Draw(_texture, _position, _rect, Color.White, 0f, Vector2.Zero, 1.0f, _spriteEffects, 0.0f);
    }
}