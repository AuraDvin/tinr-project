using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.UI;

public class UIButton : GameObject, IUiDrawableComponent, Subject {
    public UIButton(Game game) : base(game) {
    }
    public string String { get; set; } = "";
    public Texture2D Texture { get; set; }
    public Vector2 TextPosition { get; set; }
    public Vector2 TexturePosition { get; set; }
    public bool Visible { get; set; }
    public new bool Enabled {get; set;} = true;
    public List<Observer> Observers { get; set; } = new();
    public Texture2D BackgroundTexture { get; set; }
    public Rectangle ButtonBounds { get; set; }

    public void AddObserver(Observer observer) {
        Observers.Add(observer);
    }

    public virtual void OnClick() {
        ((Subject)this).Notify();
    }

    public void RemoveObserver(Observer observer) {
        Observers.Remove(observer);
    }
}
