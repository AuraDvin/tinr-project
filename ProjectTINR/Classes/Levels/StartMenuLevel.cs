using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR.Classes.Graphics;
using ProjectTINR.Classes.NPCs;
using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.UI;

namespace ProjectTINR.Classes.Levels;

class StartObserver : Observer {
    protected Game _game;

    public StartObserver(Game game) {
        _game = game;
    }

    public virtual void Notify() {
        // Access the Game instance and switch level
        if (_game is ProjectTinr projectTinr) {
            projectTinr.SwitchLevel(LevelType.MainLevel);
            // // Use reflection to call the private SwitchLevel method
            // var method = typeof(ProjectTinr).GetMethod("SwitchLevel", 
            //     System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            // method?.Invoke(projectTinr, new object[] { LevelType.MainLevel });
        }
    }

    public void Notify(string message, object args) {
        throw new NotImplementedException();
    }
}

class SettingsObserver : StartObserver {
    public SettingsObserver(Game game) : base(game) {
    }
    public override void Notify() {
        // Access the Game instance and open settings menu
        if (_game is ProjectTinr projectTinr) {
            projectTinr.SwitchLevel(LevelType.Settings);
            // // Use reflection to call the private OpenSettingsMenu method
            // var method = typeof(ProjectTinr).GetMethod("SwitchLevel", 
            //     System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            // method?.Invoke(projectTinr, new object[] { LevelType.Settings } );
        }
    }
}

class ContinueObserver(Game game) : StartObserver(game) {
    public override void Notify() {
        // Access the Game instance and continue the game
        if (_game is ProjectTinr projectTinr) {
            // Use reflection to call the private ContinueGame method
            projectTinr.SwitchLevel(LevelType.LevelSelect);
            // var method = typeof(ProjectTinr).GetMethod("SwitchLevel", 
            //     System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            // method?.Invoke(projectTinr, new object[] { LevelType.LevelSelect });
        }
    }
}

class ExitObserver(Game game) : StartObserver(game) {
    public override void Notify() {
        // Access the Game instance and exit the game
        if (_game is ProjectTinr projectTinr) {
            projectTinr.Exit();
        }
    }
}

public class StartMenuLevel : Level {
    private UIButton _startButton;
    private UIVerticalList _menuList;
    private bool _menuVisible = false;
    private KeyboardState _previousKeyboardState;
    private Observer _startObserver;

    int indexSelected = 0;

    public StartMenuLevel(Game game) : base(game) {
        _startButton = new(Game, "Start", "");
        _menuList = new(Game);
        _startObserver = new StartObserver(Game);
        _previousKeyboardState = Keyboard.GetState();
    }

    public override void Initialize() {
        base.Initialize();

        // Initialize scenes
        _scene = new Scene();
        _uiScene = new Scene();

        // Setup start button
        _startButton.Position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - 50,
                                                Game.GraphicsDevice.Viewport.Height / 2 - 25);
        _startButton.Visible = true;
        _uiScene.Add(_startButton);

        // Setup menu buttons (e.g., New Game, Continue, Settings, Exit)
        string[] menuLabels = { "New Game", "Continue", "Settings", "Exit" };
        foreach (string label in menuLabels) {
            UIButton button = new(Game, label, "") {
                Visible = false
            };

            switch (label) {
                case "New Game":
                    button.AddObserver(_startObserver);
                    break;
                case "Continue":
                    button.AddObserver(new ContinueObserver(Game));
                    break;
                case "Settings":
                    button.AddObserver(new SettingsObserver(Game));
                    break;
                case "Exit":
                    button.AddObserver(new ExitObserver(Game));
                    break;
            }

            _menuList.Children.Add(button);
            // _uiScene.Add(button);
        }

        // Initialize horizontal list for menu
        Vector2 menuPosition = new Vector2(
            Game.GraphicsDevice.Viewport.Width / 2,
            Game.GraphicsDevice.Viewport.Height / 2 + 50);
        _menuList.Position = menuPosition;
        _menuList.Hide();

        _uiScene.Add(_menuList);

        Reset();
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);

        KeyboardState currentKeyboardState = Keyboard.GetState();
        Console.WriteLine($"Selected item is {indexSelected} ");
        if (_menuList.Visible) {
            _menuList.Update(gameTime);

            if (currentKeyboardState.IsKeyUp(Keys.Up) && _previousKeyboardState.IsKeyDown(Keys.Up)) {
                indexSelected -= 1;
                if (indexSelected < 0) indexSelected = _menuList.Children.Count - 1;
            }

            if (currentKeyboardState.IsKeyUp(Keys.Down) && _previousKeyboardState.IsKeyDown(Keys.Down)) {
                indexSelected += 1;
                indexSelected %= _menuList.Children.Count;
            }

            if (currentKeyboardState.IsKeyUp(Keys.Escape) && _previousKeyboardState.IsKeyDown(Keys.Escape)) {
                _menuList.Hide();
                _startButton.Visible = true;
            }

            if (currentKeyboardState.IsKeyUp(Keys.Enter) && _previousKeyboardState.IsKeyDown(Keys.Enter) ||
                currentKeyboardState.IsKeyUp(Keys.Space) && _previousKeyboardState.IsKeyDown(Keys.Space)
            ) {
                (_menuList.Children[indexSelected] as UIButton).OnClick();
            }
        }
        else {
            // Check if start button is pressed
            if (currentKeyboardState.IsKeyUp(Keys.Enter) && _previousKeyboardState.IsKeyDown(Keys.Enter)) {
                ShowMenu();
                _startButton.Visible = false;
            }
        }

        _previousKeyboardState = currentKeyboardState;
    }

    private void ShowMenu() {
        _menuVisible = true;
        _startButton.Visible = false;
        _menuList.Show();
    }

    private void HideMenu() {
        _menuVisible = false;
        _startButton.Visible = true;
        _menuList.Hide();
    }

    public override void Reset() {
        if (_menuVisible) {
            HideMenu();
        }
    }
}