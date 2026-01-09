using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Graphics;

public class UICheckbox : GameObject, IUiDrawableComponent {
    public string Label { get; set; } = "";
    public bool Checked { get; set; } = false;
    private Vector2 _pos = Vector2.Zero;

    public UICheckbox(Game game) : base(game) { }

    // String to be drawn by the UiRenderer2D
    public string String { get => $"{Label} [{(Checked ? 'X' : ' ')}]"; set { } }
    public Texture2D Texture { get; set; } = null;
    public Vector2 TextPosition { get => _pos; set => _pos = value; }
    public Vector2 TexturePosition { get; set; } = Vector2.Zero;
    public bool Visible { get; set; } = true;

    public void Toggle() {
        Checked = !Checked;
    }
}
