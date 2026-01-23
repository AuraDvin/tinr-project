using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Sound;

public class GameSoundController : GameComponent {
    private Level _level;
    private Dictionary<ISoundPlayer, SoundController> _controllers = [];

    public GameSoundController(Game game, Level level) : base(game) {
        _level = level;
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        foreach (GameObject obj in _level.Scene) {
            if (obj is not ISoundPlayer soundPlayer) {
                continue;
            }

            SoundController soundController;
            if (!_controllers.TryGetValue(soundPlayer, out soundController)) {
                soundController = SoundControllerFactory.GetSoundController(Game, obj);
                _controllers.Add(soundPlayer, soundController);
            }
            soundController.Update(gameTime);
        }
    }
}
