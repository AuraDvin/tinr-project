using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProjectTINR.Classes.Graphics;

namespace ProjectTINR.Classes.Levels;

public class SettingsLevel : Level {
    private UICheckbox _debugCheckbox;
    private UISlider _masterVolumeSlider;
    private UISlider _musicVolumeSlider;
    private UISlider _sfxVolumeSlider;
    private List<IUiDrawableComponent> _controls = new();
    private int _selectedIndex = 0;
    private KeyboardState _prevKb;

    private float _inputDelay = 0.2f;
    private float _inputTimer = 0f;

    public SettingsLevel(Game game) : base(game) {
        _levelType = LevelType.Settings;
        _scene = new();
        _uiScene = new();
    }

    public override void Initialize() {
        base.Initialize();

        _debugCheckbox = new UICheckbox(Game) {
            Label = "Debug Physics",
            Checked = GameSettings.DebugPhysicsCollisions,
            TextPosition = new Vector2(10, 10)
        };

        _masterVolumeSlider = new UISlider(Game) {
            Label = "Master Volume",
            Value = GameSettings.MasterVolume,
            TextPosition = new Vector2(10, 50)
        };

        _musicVolumeSlider = new UISlider(Game) {
            Label = "Music Volume",
            Value = GameSettings.MusicVolume,
            TextPosition = new Vector2(10, 90)
        };  

        _sfxVolumeSlider = new UISlider(Game) {
            Label = "SFX Volume",
            Value = GameSettings.SfxVolume,
            TextPosition = new Vector2(10, 130)
        };


        _controls.Add(_debugCheckbox);
        _controls.Add(_masterVolumeSlider);
        _controls.Add(_musicVolumeSlider);
        _controls.Add(_sfxVolumeSlider);

        // Add to UI scene for rendering
        _uiScene.Add(_debugCheckbox);
        _uiScene.Add(_masterVolumeSlider);
        _uiScene.Add(_musicVolumeSlider);
        _uiScene.Add(_sfxVolumeSlider);

        // Add to Game.Components so their Update runs when needed (if they need it later)
        Game.Components.Add((IGameComponent)_debugCheckbox);
        Game.Components.Add((IGameComponent)_masterVolumeSlider);
        Game.Components.Add((IGameComponent)_musicVolumeSlider);
        Game.Components.Add((IGameComponent)_sfxVolumeSlider);

        // Set starting focus
        _selectedIndex = 0;

        System.Console.WriteLine("Settings level initialized.");
    }

    public override void Update(GameTime gameTime) {
        _inputTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        var kb = Keyboard.GetState();

        // Navigate up/down
        if (IsKeyPressed(kb, Keys.Down)) {
            _selectedIndex = (_selectedIndex + 1) % _controls.Count;
        }
        if (IsKeyPressed(kb, Keys.Up)) {
            _selectedIndex = (_selectedIndex - 1 + _controls.Count) % _controls.Count;
        }

        // Interact with selected control
        var selected = _controls[_selectedIndex];
        if (selected == _debugCheckbox) {
            if (IsKeyPressed(kb, Keys.Space) || IsKeyPressed(kb, Keys.Enter)) {
                _debugCheckbox.Toggle();
                GameSettings.DebugPhysicsCollisions = _debugCheckbox.Checked;
            }
        } else if (selected is UISlider slider) {
            bool change = false;
            if (kb.IsKeyDown(Keys.Right) || kb.IsKeyDown(Keys.D)) {
                if (_inputTimer <= 0f) {
                    slider.Increase();
                    change = true;
                    _inputTimer = _inputDelay;
                }
            } else if (kb.IsKeyDown(Keys.Left) || kb.IsKeyDown(Keys.A)) {
                if (_inputTimer <= 0f) {
                    slider.Decrease();
                    change = true;
                    _inputTimer = _inputDelay;
                }
            }
            int keysReleased = 0;
            foreach (Keys key in new List<Keys>{ Keys.Right, Keys.D, Keys.Left, Keys.A }) {
                if (kb.IsKeyUp(key)) {
                    keysReleased++;
                }
            }
            if (keysReleased == 4) {
                _inputTimer = 0f; // reset timer when all relevant keys are released
            }
            
            if (change) {
                switch (slider.Label.TrimStart('>', ' ')) {
                    case "Master Volume":
                        GameSettings.MasterVolume = slider.Value;
                        break;
                    case "Music Volume":
                        GameSettings.MusicVolume = slider.Value;
                        break;
                    case "SFX Volume":
                        GameSettings.SfxVolume = slider.Value;
                        break;
                }
            }
        }

        // Visual feedback in labels for focused control: prepend '>'
        for (int i = 0; i < _controls.Count; i++) {
            if (_controls[i] is UICheckbox uh) {
                uh.String = null; // setter does nothing, keep behavior - we'll modify label when drawing by prefixing
                // Build label with focus prefix
                var prefix = (i == _selectedIndex) ? "> " : "  ";
                uh.Label = prefix + (uh.Label.TrimStart('>', ' '));
            }
            if (_controls[i] is UISlider us) {
                var prefix = (i == _selectedIndex) ? "> " : "  ";
                us.Label = prefix + us.Label.TrimStart('>', ' ');
            }
        }

        _prevKb = kb;

        base.Update(gameTime);
    }

    private bool IsKeyPressed(KeyboardState kb, Keys key) {
        return kb.IsKeyDown(key) && !_prevKb.IsKeyDown(key);
    } 

    public override void Reset() {
        base.Reset();
    }
}
