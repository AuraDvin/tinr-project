using System;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.Objects;

public class HealPickup : PickupObject {
    public override PickupType Type { get => PickupType.HEAL; set {} }
    public HealPickup(Game game) : base(game) {
    }
    public override void Collected() {
        Scene.FindByType<Player>().HealDamage();
        base.Collected();
    }
}
