using System;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes;

public class Level(Game game) : GameComponent(game) {
    protected Scene _scene = [];
    protected Scene _uiScene = [];
    protected LevelType _levelType;
    public virtual LevelType Type => _levelType;

    public Scene Scene {
        get => _scene;
        set => _scene = value;
    }

    public Scene UIScene {
        get => _uiScene;
        set => _uiScene = value;
    }
    
    public override void Initialize() {
        // Console.WriteLine("Loading level.");
        base.Initialize();
        Reset();
    }

    public virtual void Reset() {
        // Console.WriteLine("Resetting level.");
    }

    protected override void Dispose(bool disposing) {
        // Console.WriteLine("Unloading level.");
        base.Dispose(disposing);
    }

    public virtual void Serialize() {
    }

    public virtual void Deserialize() {
    }

}
