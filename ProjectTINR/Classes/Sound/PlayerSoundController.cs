using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Sound;

public class PlayerSoundController : SoundController {
    private PlayerState _oldState = PlayerState.None;
    private bool _oldOnFloor = false;
    private readonly SoundEffect _jumpEffect;
    private readonly SoundEffect _landingEffect;
    public PlayerSoundController(Game game, GameObject owner) : base(game, owner) {
        _jumpEffect = Game.Content.Load<SoundEffect>("audio/sounds/jump");
        _landingEffect = Game.Content.Load<SoundEffect>("audio/sounds/land-on-ground");
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        Player player = Owner as Player ?? throw new Exception("Owner of PlayerSoundController is not the player");

        PlayerState currentState = player.State;

        if (currentState == PlayerState.Jumping && (player.OnFloor || player.OnWall)) {
            SoundEffectInstance soundEffectInstance = _jumpEffect.CreateInstance();
            soundEffectInstance.IsLooped = false; 
            soundEffectInstance.Volume = GameSettings.SfxVolume;
            SoundState state = soundEffectInstance.State;
            if (state != SoundState.Playing) {
                soundEffectInstance.Play();
            }
        }

        if (!_oldOnFloor && player.OnFloor) {
            SoundEffectInstance soundEffectInstance = _landingEffect.CreateInstance();
            soundEffectInstance.IsLooped = false; 
            soundEffectInstance.Volume = GameSettings.SfxVolume;
            SoundState state = soundEffectInstance.State;
            if (state != SoundState.Playing) {
                soundEffectInstance.Play();
            }
        }

        _oldOnFloor = player.OnFloor;
        _oldState = currentState;
    }
}
