using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.Levels;

namespace ProjectTINR.Classes;

public class LevelFactory {
    public static Level CreateLevel(Game game, LevelType levelType) {
        return levelType switch {
            LevelType.MainLevel => new MainLevel(game),
            LevelType.Settings => new SettingsLevel(game),
            LevelType.StartMenu => new StartMenuLevel(game),
            LevelType.LevelSelect => new SelectLevelLevel(game, 6),
            LevelType.LevelComplete => new LevelCompleteLevel(game),
            LevelType.Credits => new CreditsLevel(game),
            _ => throw new ArgumentOutOfRangeException(nameof(levelType), "Invalid level type")
        };
    }
}
