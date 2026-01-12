using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProjectTINR.Classes.UI;

namespace ProjectTINR.Classes.Levels;

class LevelObserver : Observer {
    ProjectTinr _game; 

    public LevelObserver(ProjectTinr game) {
        _game = game;
    }

    public void Notify() {
        Console.WriteLine("A level button was clicked.");
        _game.SwitchLevel(LevelType.StartMenu);
    }

    public void Notify(string message, object args) {
        throw new NotImplementedException();
    }

    public void OnNotify(int levelIndx) {
        Console.WriteLine($"Level {levelIndx} selected.");
        // Tell game to load the right level layout

        _game.SwitchLevel(LevelType.MainLevel);
    }
}

class CustomButton : UIButton {
    private int _levelIndex;
    private LevelObserver _observer;

    public CustomButton(Game game, int levelIndex, LevelObserver observer) :   base(game, levelIndex.ToString(), "") {
        _levelIndex = levelIndex;
        AddObserver(observer);
    }

    public override void OnClick() {
        base.OnClick();
        _observer.OnNotify(_levelIndex);
    }
}

public class SelectLevelLevel : Level {
    public LevelType _levelType = LevelType.LevelSelect;
    private float _inputDelay = 0.2f;
    private float _timeSinceLastInput = 0f;
    private UIHorizontalList _levelRow1, _levelRow2;
    private int _levelSelected = 0; 
    private int _rowSelected = 0;
    private int _numberOfLevels = 10;
    private KeyboardState _lastKB;
    LevelObserver _levelObserver;
    public SelectLevelLevel(Game game, int numberOfLevels) : base(game) {
        _scene = new Scene();
        _uiScene = new Scene();
        _numberOfLevels = numberOfLevels;

        _levelObserver = new LevelObserver(game as ProjectTinr);

        // Initialize level rows
        _levelRow1 = new UIHorizontalList(game);
        _levelRow2 = new UIHorizontalList(game);

        // Add level buttons to rows
        for (int i = 0; i < numberOfLevels; i++) {
            // todo: if level is locked, create a locked button instead
            CustomButton button = new(game, i, _levelObserver) {
                String = $"{i + 1}",
                Visible = true,
            };

            if (i < numberOfLevels / 2) {
                _levelRow1.Children.Add(button);
            } else {
                _levelRow2.Children.Add(button);
            }
        }

        // Position the rows
        Vector2 row1Position = new Vector2(
            Game.GraphicsDevice.Viewport.Width / 2 - 100,
            Game.GraphicsDevice.Viewport.Height / 2 - 50);
        Vector2 row2Position = new Vector2(
            Game.GraphicsDevice.Viewport.Width / 2 - 100,
            Game.GraphicsDevice.Viewport.Height / 2 + 50);

        _levelRow1.Position = row1Position;
        _levelRow2.Position = row2Position;
        _uiScene.Add(_levelRow1);
        _uiScene.Add(_levelRow2);

        Console.WriteLine("SelectLevelLevel initialized with ", numberOfLevels, " levels.");
        _levelRow1.Visible = true;
        _levelRow2.Visible = true;
    }
    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        // KeyboardState kb = Keyboard.GetState();
        // _timeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
        // if (_timeSinceLastInput < _inputDelay) {
        //     return;
        // }

        // if (Pressed(kb, Keys.Escape)) {
        //     // Handle escape key to go back to main menu
        //     _levelObserver.Notify();
        //     _timeSinceLastInput = 0f;
        // }

        // if (Released(kb, Keys.Up) || Released(kb, Keys.W)){
        //     _rowSelected = (_rowSelected + 1) % 2;
        //     if (_rowSelected == 0) {
        //         _levelRow2.Enabled = false;
        //         _levelRow1.Enabled = true;
        //     } else {
        //         _levelRow1.Enabled = false;
        //         _levelRow2.Enabled = true;
        //     }
        //     _timeSinceLastInput = 0f;
        // }

        // if (Released(kb, Keys.Down) || Released(kb, Keys.S)){
        //     _rowSelected = _rowSelected == 0 ? 1 : 0;
        //     if (_rowSelected == 0) {
        //         _levelRow2.Enabled = false;
        //         _levelRow1.Enabled = true;
        //     } else {
        //         _levelRow1.Enabled = false;
        //         _levelRow2.Enabled = true;
        //     }
        //     _timeSinceLastInput = 0f;
        // }

        // if (Released(kb, Keys.Enter) || Released(kb, Keys.Space)){
        //     // Load the selected level
        //     if (_rowSelected == 0) {
        //         _levelRow1.ActivateSelected();
        //         // Console.WriteLine($"Loading Level {levelIndex + 1} from Row 1");
        //     } else {
        //         _levelRow2.ActivateSelected();
        //         // int levelIndex = _levelRow2.GetSelectedIndex() + _numberOfLevels / 2;
        //         // Console.WriteLine($"Loading Level {levelIndex + 1} from Row 2");
        //     }
        //     _timeSinceLastInput = 0f;
        // }

        // if (kb.IsKeyDown(Keys.Left)) {
        //     _levelRow1.SelectPrevious();
        //     _levelRow2.SelectPrevious();
        //     _timeSinceLastInput = 0f;
        // }

        // if (kb.IsKeyDown(Keys.Right)) {
        //     _levelRow2.SelectNext();
        //     _levelRow1.SelectNext();
        //     _timeSinceLastInput = 0f;
        // }


        // _lastKB = kb;
    }
    
    private bool Pressed(KeyboardState currrentKB, Keys key) {
        return currrentKB.IsKeyDown(key) && !_lastKB.IsKeyDown(key);
    }
    private bool Released(KeyboardState currrentKB, Keys key) {
        return !currrentKB.IsKeyDown(key) && _lastKB.IsKeyDown(key);
    }
}
