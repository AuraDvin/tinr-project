using System.Collections.Generic;

using ProjectTINR.Classes.ObjectsComponents;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR.Classes;
using System;

namespace TINR.Classes;

public class Sprite : IGameComponent, IDrawableGameComponent, PositionComp {
    protected Rectangle _rect;
    static Texture2D _texture;
    protected Vector2 _position;

    public Vector2 Position {
        get { return _position; }
        set { _position = value; }
    }

    public Sprite(Rectangle rect, Vector2 position, Texture2D texture)
    // : base(game) 
    {
        _rect = rect;
        _position = position;
        _texture = texture;
    }

    public void SetRect(Rectangle rect) {
        _rect = rect;
    }

    public void Initialize() {
    }

    public void Draw(SpriteBatch sp) {
        Console.WriteLine($"Sprite - Draw at position {_position}");
        sp.Draw(_texture, _position, _rect, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
    }
}