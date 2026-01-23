using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Sound;

public class SoundControllerFactory {
    public static SoundController GetSoundController(Game game, GameObject gameObject) {
        if (gameObject is Player player) {
            return new PlayerSoundController(game, player);
        }

        return null;
    }
}
