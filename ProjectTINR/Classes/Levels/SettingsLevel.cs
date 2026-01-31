using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.Marshalling;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProjectTINR.Classes.Graphics;
using ProjectTINR.Classes.UI;

namespace ProjectTINR.Classes.Levels;

public class SettingsLevel : Level {
    private UIVerticalList _uIVerticalList;
    private UICheckbox _debugCheckbox;
    private UISlider _masterVolumeSlider;
    private UISlider _musicVolumeSlider;
    private UISlider _sfxVolumeSlider;
    // private List<IUiDrawableComponent> _controls = new();
    private int _selectedIndex = 0;
    private KeyboardState _prevKb;

    private float _inputDelay = 0.2f;
    private float _inputTimer = 0f;

    public SettingsLevel(Game game) : base(game) {
        _levelType = LevelType.Settings;
        _scene = new();
        _uiScene = new();
        _uIVerticalList = new(game) {
            Position = new Vector2(10, 10)
        };
    }

    public override void Initialize() {
        base.Initialize();

        _debugCheckbox = new UICheckbox(Game, "Debug Physics Off", "Debug Physics On", "") {
            Checked = GameSettings.DebugPhysicsCollisions,
        };

        _masterVolumeSlider = new UISlider(Game, "Master Volume", "") {
            Value = GameSettings.MasterVolume,
        };

        _musicVolumeSlider = new UISlider(Game, "Music Volume", "") {
            Value = GameSettings.MusicVolume,
        };  

        _sfxVolumeSlider = new UISlider(Game, "SFX Volume", "") {
            Value = GameSettings.SfxVolume,
        };


        _uIVerticalList.Children.Add(_debugCheckbox);
        _uIVerticalList.Children.Add(_masterVolumeSlider);
        _uIVerticalList.Children.Add(_musicVolumeSlider);
        _uIVerticalList.Children.Add(_sfxVolumeSlider);

        // Add to UI scene for rendering
        _uiScene.Add(_uIVerticalList);
        // _uiScene.Add(_debugCheckbox);
        // _uiScene.Add(_masterVolumeSlider);
        // _uiScene.Add(_musicVolumeSlider);
        // _uiScene.Add(_sfxVolumeSlider);

        // Add to Game.Components so their Update runs when needed (if they need it later)
        // Game.Components.Add((IGameComponent)_debugCheckbox);
        // Game.Components.Add((IGameComponent)_masterVolumeSlider);
        // Game.Components.Add((IGameComponent)_musicVolumeSlider);
        // Game.Components.Add((IGameComponent)_sfxVolumeSlider);

        // Set starting focus
        _selectedIndex = 0;

        System.Console.WriteLine("Settings level initialized.");
    }

    public override void Update(GameTime gameTime) {
        _inputTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        var kb = Keyboard.GetState();
        Console.WriteLine($"Selected item: {_selectedIndex}");
        // Navigate up/down
        if (IsKeyPressed(kb, Keys.Down)) {
            (_uIVerticalList.Children[_selectedIndex] as SimpleUIElement).Selected = false;
            _selectedIndex = (_selectedIndex + 1) % _uIVerticalList.Children.Count;
        }
        if (IsKeyPressed(kb, Keys.Up)) {
            (_uIVerticalList.Children[_selectedIndex] as SimpleUIElement).Selected = false;
            _selectedIndex = (_selectedIndex - 1 + _uIVerticalList.Children.Count) % _uIVerticalList.Children.Count;
        }

        (_uIVerticalList.Children[_selectedIndex] as SimpleUIElement).Selected = true;

        // Interact with selected control
        var selected = _uIVerticalList.Children[_selectedIndex];
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
                Console.WriteLine($"Change {slider.String}");
                if (slider.String.StartsWith("Master")) {
                    GameSettings.MasterVolume = slider.Value; 
                } else if (slider.String.StartsWith("Music")) {
                    GameSettings.MusicVolume = slider.Value;
                }else if (slider.String.StartsWith("SFX")) {
                    GameSettings.SfxVolume = slider.Value;
                } else {
                    Console.WriteLine("Something went wrong");
                }
                // switch (slider.String.TrimStart('>', ' ')) {
                //     case "Master Volume":
                //         GameSettings.MasterVolume = slider.Value;
                //         Console.WriteLine($"Updated Master volume {slider.Value}");
                //         break;
                //     case "Music Volume":
                //         GameSettings.MusicVolume = slider.Value;
                //         Console.WriteLine($"Updated Music volume {slider.Value}");
                //         break;
                //     case "SFX Volume":
                //         GameSettings.SfxVolume = slider.Value;
                //         Console.WriteLine($"Updated SFX volume {slider.Value}");
                //         break;
                // }
            }
        }

        // Visual feedback in labels for focused control: prepend '>'
        // for (int i = 0; i < _controls.Count; i++) {
        //     if (_controls[i] is UICheckbox uh) {
        //         uh.String = null; // setter does nothing, keep behavior - we'll modify label when drawing by prefixing
        //         // Build label with focus prefix
        //         var prefix = (i == _selectedIndex) ? "> " : "  ";
        //         uh.String = prefix + (uh.String.TrimStart('>', ' '));
        //     }
        //     if (_controls[i] is UISlider us) {
        //         var prefix = (i == _selectedIndex) ? "> " : "  ";
        //         us.String = prefix + us.String.TrimStart('>', ' ');
        //     }
        // }

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
