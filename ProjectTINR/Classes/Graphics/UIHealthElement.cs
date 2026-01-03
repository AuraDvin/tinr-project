using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR.Classes.Objects;

namespace ProjectTINR.Classes.Graphics;

public class UIHealthElement : IUiDrawableComponent {
    private Vector2 _pos = new(0,0);
    public Player Player {get; set;} = null;
    public string String { get => "" + Player.Health; set {} }
    public Texture2D Texture { get; set; } = null;
    public Vector2 TextPosition { get => _pos; set {} }
    public bool Visible { get; set; } = true;
    public Vector2 TexturePosition { get; set; } = Vector2.Zero;
}
