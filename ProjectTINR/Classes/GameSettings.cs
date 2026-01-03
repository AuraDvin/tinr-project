namespace ProjectTINR.Classes;

public static class GameSettings {
    // Whether to render collision debug shapes
    public static bool DebugPhysicsCollisions { get; set; } = false;

    // Master volume from 0.0 to 1.0
    private static float _volume = 1.0f;
    public static float Volume {
        get => _volume;
        set {
            if (value < 0f) _volume = 0f;
            else if (value > 1f) _volume = 1f;
            else _volume = value;
        }
    }
}