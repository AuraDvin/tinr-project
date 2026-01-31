using System;
using System.Runtime.CompilerServices;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.Objects;

public class ShootFasterPickup : PickupObject {
    public override PickupType Type { get => PickupType.SHOOT_SPEED; set { } }
    public ShootFasterPickup(Game game) : base(game) {
    }
    public override void Collected() {
        // Notify that it should lower attack rate
        Scene.FindByType<Player>().IncreaseProjectileSpeed();
        base.Collected();
    }
}
