using System;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes;

public class Level : GameComponent {
    protected Scene _scene;
    protected LevelType _levelType;

    public Level(Game game) : base(game) {
    }

    public LevelType Type => _levelType;

    public Scene Scene {
        get => _scene;
        set => _scene = value;
    }

    public override void Initialize() {
        Console.WriteLine("Loading level.");
        base.Initialize();
        Reset();
    }

    public virtual void Reset() {
        Console.WriteLine("Resetting level.");
    }

    protected override void Dispose(bool disposing) {
        Console.WriteLine("Unloading level.");
        base.Dispose(disposing);
    }

}
