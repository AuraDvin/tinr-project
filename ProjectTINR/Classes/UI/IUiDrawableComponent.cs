using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectTINR.Classes.UI;

public interface IUiDrawableComponent {
    // Gives you if the Ui Element has a Label
    public string String {get; set;}
    // Gives you icon/texture if existing
    public Texture2D Texture { get; set; }
    public Vector2 TextPosition {get; set;}
    public Vector2 TexturePosition {get; set;}
    // If it should be drawn or not
    public bool Visible { get; set; }

}
