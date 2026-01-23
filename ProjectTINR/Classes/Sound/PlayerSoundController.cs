using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Sound;

public class PlayerSoundController : SoundController {
    private PlayerState _oldState = PlayerState.None;
    private SoundEffect _jumpEffect;
    public PlayerSoundController(Game game, GameObject owner) : base(game, owner) {
        _jumpEffect = Game.Content.Load<SoundEffect>("audio/sounds/jump");
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        Player player = Owner as Player ?? throw new Exception("Owner of PlayerSoundController is not the player");

        PlayerState currentState = player.State;

        // Only updating on state change
        if (_oldState == currentState) {
            return;
        }

        if (currentState == PlayerState.Jumping) {
            SoundEffectInstance soundEffectInstance = _jumpEffect.CreateInstance();
            soundEffectInstance.IsLooped = false; 
            soundEffectInstance.Volume = GameSettings.SfxVolume;
            SoundState state = soundEffectInstance.State;
            if (state != SoundState.Playing) {
                soundEffectInstance.Play();
            }
        }
        if (_oldState == PlayerState.Falling && player.OnFloor) {
            SoundEffectInstance soundEffectInstance = _jumpEffect.CreateInstance();
            soundEffectInstance.IsLooped = false; 
            soundEffectInstance.Volume = GameSettings.SfxVolume;
            SoundState state = soundEffectInstance.State;
            if (state != SoundState.Playing) {
                soundEffectInstance.Play();
            }
        }

        _oldState = currentState;
    }
}
