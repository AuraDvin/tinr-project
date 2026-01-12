using System;
using System.Reflection.Emit;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR.Classes.Objects;

namespace ProjectTINR.Classes.UI;

public class UIHealthElement : UiLabel {
    public UIHealthElement(Game game, string str, string textureName) : base(game, str, textureName) {
        Position = new(0, 0);
    }
    public Player Player { get; set; }
    public override string String { get => base.String; set => base.String = value; }
    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        String = Player.Health.ToString();
    }
    // private Vector2 _pos = new(0,0);
    // private Game _game;

    // public UIHealthElement(Game game) {
    //     _game = game;
    // }

    // public Player Player {get; set;} = null;
    // public string String { get => "" + Player.Health; set {} }
    // public Texture2D Texture { get; set; } = null;
    // public Vector2 TextPosition { get => _pos; set {} }
    // public bool Visible { get; set; } = true;
    // public Vector2 TexturePosition { get; set; } = Vector2.Zero;

    // public string Name => throw new NotImplementedException();

    // public Vector2 Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    // public void Initialize() {
    //     throw new NotImplementedException();
    // }
}
