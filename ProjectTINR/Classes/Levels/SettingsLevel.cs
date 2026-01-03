using System.Collections.Generic;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProjectTINR.Classes.Graphics;

namespace ProjectTINR.Classes.Levels;

public class SettingsLevel : Level {
    private UICheckbox _debugCheckbox;
    private UISlider _volumeSlider;
    private List<IUiDrawableComponent> _controls = new();
    private int _selectedIndex = 0;
    private KeyboardState _prevKb;

    public SettingsLevel(Game game) : base(game) {
        _levelType = LevelType.Settings;
        _scene = new();
        _uiScene = new();
    }

    public override void Initialize() {
        base.Initialize();

        _debugCheckbox = new UICheckbox(Game) {
            Label = "Debug Physics",
            Checked = ProjectTINR.Classes.GameSettings.DebugPhysicsCollisions,
            TextPosition = new Vector2(10, 10)
        };

        _volumeSlider = new UISlider(Game) {
            Label = "Volume",
            Value = ProjectTINR.Classes.GameSettings.Volume,
            TextPosition = new Vector2(10, 40)
        };

        _controls.Add(_debugCheckbox);
        _controls.Add(_volumeSlider);

        // Add to UI scene for rendering
        _uiScene.Add(_debugCheckbox);
        _uiScene.Add(_volumeSlider);

        // Add to Game.Components so their Update runs when needed (if they need it later)
        Game.Components.Add((IGameComponent)_debugCheckbox);
        Game.Components.Add((IGameComponent)_volumeSlider);

        // Set starting focus
        _selectedIndex = 0;

        System.Console.WriteLine("Settings level initialized.");
    }

    public override void Update(GameTime gameTime) {
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
                ProjectTINR.Classes.GameSettings.DebugPhysicsCollisions = _debugCheckbox.Checked;
            }
        } else if (selected == _volumeSlider) {
            if (IsKeyPressed(kb, Keys.Right) || IsKeyPressed(kb, Keys.D)) {
                _volumeSlider.Increase();
                ProjectTINR.Classes.GameSettings.Volume = _volumeSlider.Value;
            }
            if (IsKeyPressed(kb, Keys.Left) || IsKeyPressed(kb, Keys.A)) {
                _volumeSlider.Decrease();
                ProjectTINR.Classes.GameSettings.Volume = _volumeSlider.Value;
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
