using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectTINR.Classes;

public static class GameSettings {
    public static bool GamePaused { get; set; } = false;
    public static bool DebugPhysicsCollisions { get; set; } = false;
    private static float _masterVolume = 1.0f;
    public static float MasterVolume {
        get => _masterVolume;
        set {
            if (value < 0f) _masterVolume = 0f;
            else if (value > 1f) _masterVolume = 1f;
            else _masterVolume = value;
        }
    }

    public static float MusicVolume { get; set; } = 1f;
    public static float SfxVolume { get; set; } = 1f;
    public static SpriteFont SpriteFont { get; set; }
}