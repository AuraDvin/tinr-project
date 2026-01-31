using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.UI;

public class UIHorizontalList : GameObject, ComplexUIElement {
    public UIHorizontalList(Game game) : base(game) {
    }

    public Scene Children { get; set; } = [];
    public bool Visible { get; set; } = true;
    public float Spacing { get; set; } = 40;
    public bool HasString => String.Length > 0;
    public bool HasTexture => TextureName.Length > 0;
    public string String { get; set; } = "";
    public string TextureName { get; set; } = "";
    public Vector2 TextPosition { get; set; } = Vector2.Zero;
    public Vector2 TexturePosition { get; set; } = Vector2.Zero;
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Rectangle TextureRect { get; set; } = new Rectangle(0, 0, 32, 32);

    public virtual bool Selectable => false;

    public virtual bool Selected { get; set; }

    public void Show() {
        Visible = true;
    }

    public void Hide() {
        Visible = false;
    }
}
