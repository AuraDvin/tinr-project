using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.UI;

public class UIButton : UiLabel, Subject {
    protected override string _prefix => "UIButton";
    public bool Selectable { get => true; }
    public virtual bool Selected { get; set; } = false;

    public UIButton(Game game, string str, string textureName) : base(game, str, textureName) {
    }
    public UIButton(Game game, string str) : base(game, str) {
    }
    public Rectangle Padding { get; set; } = new Rectangle(10, 10, 10, 10);

    public List<Observer> Observers { get; set; } = new();

    public void AddObserver(Observer observer) {
        Observers.Add(observer);
    }

    public virtual void OnClick() {
        ((Subject)this).Notify();
    }

    public virtual void OnClick(string message, object? args) {
        ((Subject)this).Notify(message, args);
    }

    public void RemoveObserver(Observer observer) {
        Observers.Remove(observer);
    }
}
