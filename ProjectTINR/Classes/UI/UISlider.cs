using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.UI;

public class UISlider : GameObject, IUiDrawableComponent {
    public string Label { get; set; } = "";
    private float _value = 1.0f;
    private Vector2 _pos = Vector2.Zero;

    public UISlider(Game game) : base(game) { }

    public float Value {
        get => _value;
        set {
            if (value < 0f) _value = 0f;
            else if (value > 1f) _value = 1f;
            else _value = value;
        }
    }

    public string String {
        get {
            int segments = 10;
            int filled = (int)System.Math.Round(Value * segments);
            string bar = new string('#', filled).PadRight(segments, '-');
            return $"{Label}: {Value:0.00} [{bar}]";
        }
        set { }
    }

    public Texture2D Texture { get; set; } = null;
    public Vector2 TextPosition { get => _pos; set => _pos = value; }
    public Vector2 TexturePosition { get; set; } = Vector2.Zero;
    public bool Visible { get; set; } = true;

    public void Increase(float step = 0.05f) {
        Value += step;
    }

    public void Decrease(float step = 0.05f) {
        Value -= step;
    }
}
