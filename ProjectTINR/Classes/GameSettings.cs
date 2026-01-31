using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.IO;
using System.Text.Json;

namespace ProjectTINR.Classes;

public static class GameSettings {
    private static readonly string s_fileName = "settings.json";
    public static bool GamePaused { get; set; } = false;
    public static bool DebugPhysicsCollisions { get; set; } = false;
    private static float _masterVolume = 1.0f;
    public static int LevelNum = 1;
    public static float MasterVolume {
        get => _masterVolume;
        set {
            if (value < 0f) _masterVolume = 0f;
            else if (value > 1f) _masterVolume = 1f;
            else _masterVolume = value;
        }
    }
    private static float s_musicVolume = 1f;
    private static float s_sfxVolume = 1f;
    public static float MusicVolume {
        get => s_musicVolume; set {
            if (value < 0f) s_musicVolume = 0f;
            else if (value > 1f) s_musicVolume = 1f;
            s_musicVolume = value;
        }
    }
    public static float SfxVolume {
        get => s_sfxVolume; set {
            if (value < 0f) s_sfxVolume = 0f;
            else if (value > 1f) s_sfxVolume = 1f;
            s_sfxVolume = value;
        }
    }
    public static SpriteFont SpriteFont { get; set; }

    public static void Initialize() {
        LoadSettings();
    }

    private static string GetSettingsPath() {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string settingsDir = Path.Combine(appDataPath, "enchanted");

        if (!Directory.Exists(settingsDir)) {
            Directory.CreateDirectory(settingsDir);
        }

        return Path.Combine(settingsDir, s_fileName);
    }

    public static void SaveSettings() {
        try {
            var settings = new {
                MasterVolume,
                MusicVolume,
                SfxVolume,
                GamePaused,
                DebugPhysicsCollisions
            };

            string path = GetSettingsPath();
            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
        catch (Exception ex) {
            Console.WriteLine($"Error saving settings: {ex.Message}");
        }
    }

    private static void LoadSettings() {
        try {
            string path = GetSettingsPath();

            if (!File.Exists(path)) {
                return;
            }

            string json = File.ReadAllText(path);
            using (JsonDocument doc = JsonDocument.Parse(json)) {
                var root = doc.RootElement;

                if (root.TryGetProperty("MasterVolume", out var masterVol)) {
                    MasterVolume = masterVol.GetSingle();
                }
                if (root.TryGetProperty("MusicVolume", out var musicVol)) {
                    MusicVolume = musicVol.GetSingle();
                }
                if (root.TryGetProperty("SfxVolume", out var sfxVol)) {
                    SfxVolume = sfxVol.GetSingle();
                }
                if (root.TryGetProperty("GamePaused", out var paused)) {
                    GamePaused = paused.GetBoolean();
                }
                if (root.TryGetProperty("DebugPhysicsCollisions", out var debug)) {
                    DebugPhysicsCollisions = debug.GetBoolean();
                }
            }
        }
        catch (Exception ex) {
            Console.WriteLine($"Error loading settings: {ex.Message}");
        }
    }
}