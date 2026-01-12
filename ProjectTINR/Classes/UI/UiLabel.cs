using System;
using System.Numerics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR.Classes.Graphics;
using ProjectTINR.Classes.ObjectsComponents;

using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProjectTINR.Classes.UI;

public class UiLabel : GameObject, SimpleUIElement {
    public virtual bool Visible { get; set; } = true;
    public new virtual bool Enabled { get; set; } = true;
    public virtual bool HasString  {get; set; } = false;
    public virtual bool HasTexture {get; set; } = false;
    protected override string _prefix => "UiLabel";
    public UiLabel(Game game, string str, string textureName) : base(game) {
        String = str;
        TextureName = textureName;
    }

    public UiLabel(Game game, string str) : base(game) {
        String = str;
    }

    public virtual string String { get => _string; set { _string = value; HasString = value.Length > 0;} }
    public string TextureName { get => _textureName; set {_textureName = value; HasTexture = value.Length > 0; } }
    public Vector2 TextPosition { get; set; } = Vector2.Zero;
    public Vector2 TexturePosition { get; set; } = Vector2.Zero;
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Rectangle TextureRect { get; set; } = new Rectangle(0, 0, 32, 32);
    protected string _string = ""; 
    protected string _textureName = ""; 
}
