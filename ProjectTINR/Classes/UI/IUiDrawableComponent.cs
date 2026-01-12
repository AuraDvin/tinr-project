using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.UI;

public interface IUiDrawableComponent : IPositionComponent {
    public bool Visible { get; set; }
    public bool Enabled { get; set; }
    public bool HasString { get; }
    public bool HasTexture { get; }
    public string String { get; set; }
    public string TextureName { get; set; }
    public Vector2 TextPosition { get; set; }
    public Vector2 TexturePosition { get; set; }
    public Rectangle TextureRect { get; set; }
}
