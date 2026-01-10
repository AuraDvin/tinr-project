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
    private Game _game;

    public StartObserver(Game game) {
        _game = game;
    }

    public void Notify() {
        // Access the Game instance and switch level
        if (_game is ProjectTinr projectTinr) {
            // Use reflection to call the private SwitchLevel method
            var method = typeof(ProjectTinr).GetMethod("SwitchLevel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method?.Invoke(projectTinr, new object[] { LevelType.MainLevel });
        }
    }
}

public class StartMenuLevel: Level {
    private UIButton _startButton;
    private List<UIButton> _menuButtons;
    private UIHorizontalList _menuList;
    private bool _menuVisible = false;
    private KeyboardState _previousKeyboardState;
    private Observer _startObserver;

    public StartMenuLevel(Game game) : base(game) {
        _startButton = new(Game);
        _menuButtons = [];
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
        _startButton.String = "START";
        _startButton.TextPosition = new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - 50, 
                                                Game.GraphicsDevice.Viewport.Height / 2 - 25);
        _startButton.Visible = true;
        _startButton.AddObserver(_startObserver);
        _uiScene.Add(_startButton);

        // Setup menu buttons (e.g., New Game, Continue, Settings, Exit)
        string[] menuLabels = { "New Game", "Continue", "Settings", "Exit" };
        foreach (string label in menuLabels) {
            UIButton button = new(Game) {
                String = label,
                Visible = false
            };
            
            if (label == "New Game") {
                button.AddObserver(_startObserver);
            }

            _menuButtons.Add(button);
            _uiScene.Add(button);
        }

        // Initialize horizontal list for menu
        Vector2 menuPosition = new Vector2(
            Game.GraphicsDevice.Viewport.Width / 2,
            Game.GraphicsDevice.Viewport.Height / 2 + 50);
        _menuList.Initialize(menuPosition, _menuButtons);
        _uiScene.Add(_menuList);

        Reset();
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);

        KeyboardState currentKeyboardState = Keyboard.GetState();

        if (_menuVisible) {
            _menuList.Update(gameTime);
        }
        else {
            // Check if start button is pressed
            if (currentKeyboardState.IsKeyUp(Keys.Enter) && _previousKeyboardState.IsKeyDown(Keys.Enter)) {
                ShowMenu();
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