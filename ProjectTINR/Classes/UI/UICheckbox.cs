using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.UI;

public class UICheckbox : UIButton {
    public bool Checked { get; set; } = false;
    public override string String { get => Checked ? _checkedstr : base.String; set => base.String = value; }
    string _checkedstr;
    public UICheckbox(Game game, string str,string checkedstr, string textureName) : base(game, str, textureName) {
        _checkedstr = checkedstr;
    }

    public void Toggle() {
        Checked = !Checked;
    }
    
    public override void OnClick() {
        Toggle();
        base.OnClick();
    }

    public override void OnClick(string message, object args) {
        Toggle();
        base.OnClick(message, args);
    }
}
