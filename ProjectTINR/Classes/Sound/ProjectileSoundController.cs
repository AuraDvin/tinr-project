using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Sound;

public class ProjectileSoundController : SoundController
{
    public string SoundPath = "";
    private bool _played = false;
    private SoundEffect _sfx;

    public ProjectileSoundController(Game game, GameObject owner, string soundPath) : base(game, owner) {
        SoundPath = soundPath;
        if (SoundPath.Length == 0) {
            throw new Exception("No audio path specified");
        }
        _sfx = Game.Content.Load<SoundEffect>(soundPath);
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        if (_played) {
            return;
        }

        SoundEffectInstance soundEffectInstance = _sfx.CreateInstance();
        soundEffectInstance.Volume = GameSettings.SfxVolume * GameSettings.MasterVolume;
        soundEffectInstance.IsLooped = false;
        soundEffectInstance.Play();
        _played = true;
    }
}
