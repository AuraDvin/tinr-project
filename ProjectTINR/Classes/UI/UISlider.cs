using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.UI;

public class UISlider : UiLabel {
    private float _value = 1.0f;

    public UISlider(Game game, string str, string textureName) : base(game, str, textureName) { }

    public float Value {
        get => _value;
        set {
            if (value < 0f) _value = 0f;
            else if (value > 1f) _value = 1f;
            else _value = value;
        }
    }
    public override string String {
        get {
            string up = base.String;
            int segments = 10;
            int filled = (int)Math.Round(Value * segments);
            string bar = new string('#', filled).PadRight(segments, '-');
            return up + " [" + bar + "]";
        }
        set {
            base.String = value;
        }
    }

    public void Increase(float step = 0.05f) {
        Value += step;
    }

    public void Decrease(float step = 0.05f) {
        Value -= step;
    }
}
