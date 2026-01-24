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
        
        if (gameObject is PlayerProjectile) {
            return new ProjectileSoundController(game, gameObject, "audio/sounds/flying-knife");
        } else if (gameObject is Projectile) {
            return new ProjectileSoundController(game, gameObject, "audio/sounds/jump");
        }
        
        return null;
    }
}
