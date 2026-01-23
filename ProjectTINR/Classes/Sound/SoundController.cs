using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Sound;

public class SoundController : GameComponent {
    protected GameObject Owner; 
    public SoundController(Game game, GameObject owner) : base(game) {
        Owner = owner; 
        if (owner == null) {
            throw new Exception("Sound controller owner can't be unset!");
        }
    }
    // Allow classes to change this function 
    public new virtual void Update(GameTime gameTime) {
        base.Update(gameTime);
    }
}
