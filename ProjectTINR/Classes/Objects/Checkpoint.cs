using System;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.Objects;

public class Checkpoint : PickupObject {
    private bool _isLast = false;
    public override PickupType Type { get => PickupType.CHECKPOINT; set { } }
    public Checkpoint(Game game, Vector2 position, bool isLast) : base(game) {
        _isLast = isLast;
        Position = position;
    }
    public override void Collected() {
        // Notify to change spawn location 
        // or if it's last checkpoint 
        // move to the level-complete screen
        Scene.FindByType<Player>().CollectCheckpoint(this);
        if (_isLast) {
            (Game as ProjectTinr).SwitchLevel(LevelType.LevelComplete);
        }
        base.Collected();
    }
}
