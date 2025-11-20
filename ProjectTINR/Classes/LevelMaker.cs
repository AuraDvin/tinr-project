using System;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes;

public class LevelMaker {
    public static Level CreateLevel(Game game, LevelType levelType) {
        // Console.WriteLine("Creating level of type: ", nameof(levelType), ", ", (int) levelType);
        return levelType switch {
            LevelType.StartMenu => new Levels.StartMenuLevel(game),
            // LevelType.StartMenu => new StartMenuLevel(game),
            // LevelType.Credits => new CreditsLevel(game),
            // LevelType.Options => new OptionsLevel(game),
            // LevelType.Gameplay => new GameplayLevel(game),
            // LevelType.PauseMenu => new PauseMenuLevel(game),
            // LevelType.GameOver => new GameOverLevel(game),
            _ => throw new ArgumentOutOfRangeException(nameof(levelType), "Invalid level type")
        };
    }
}
