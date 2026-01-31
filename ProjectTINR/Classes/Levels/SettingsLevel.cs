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

        // Set starting focus
        _selectedIndex = 0;

        // System.Console.WriteLine("Settings level initialized.");
    }

    public override void Update(GameTime gameTime) {
        _inputTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        var kb = Keyboard.GetState();
        // Console.WriteLine($"Selected item: {_selectedIndex}");
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
                _inputTimer = 0f;
            }
            
            if (change) {
                // Console.WriteLine($"Change {slider.String}");
                if (slider.String.StartsWith("Master")) {
                    GameSettings.MasterVolume = slider.Value; 
                } else if (slider.String.StartsWith("Music")) {
                    GameSettings.MusicVolume = slider.Value;
                }else if (slider.String.StartsWith("SFX")) {
                    GameSettings.SfxVolume = slider.Value;
                } else {
                    Console.WriteLine("Something went wrong");
                }
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
