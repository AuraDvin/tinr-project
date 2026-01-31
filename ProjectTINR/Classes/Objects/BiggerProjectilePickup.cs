using System;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.Objects;

public class BiggerProjectilePickup : PickupObject {
    public override PickupType Type { get => PickupType.BIGGER_PROJECTILE; set { } }
    public BiggerProjectilePickup(Game game) : base(game) {
    }
    public override void Collected() {
        // Notify to make projectiles bigger
        Scene.FindByType<Player>().IncreaseProjectileSize();
        base.Collected();
    }
}
