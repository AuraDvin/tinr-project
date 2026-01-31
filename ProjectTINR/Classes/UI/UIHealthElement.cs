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
}
