using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjectTINR.Classes.UI;

namespace ProjectTINR.Classes.Levels;

class LevelObserver : Observer {
    readonly ProjectTinr _game;

    public LevelObserver(ProjectTinr game) {
        _game = game;
    }

    public void Notify() {
        // Console.WriteLine("A level button was clicked.");
        _game.SwitchLevel(LevelType.StartMenu);
    }

    public void Notify(string message, object args) {
        throw new NotImplementedException();
    }

    public void OnNotify(int levelIndx) {
        // Console.WriteLine($"Level {levelIndx} selected.");
        // Tell game to load the right level layout
        GameSettings.LevelNum = levelIndx;
        LevelDataManager.Instance.ReadData($"Content/levels/level{levelIndx}.json");
        _game.SwitchLevel(LevelType.MainLevel);
    }
}

class CustomButton : UIButton {
    private int _levelIndex;
    private LevelObserver _observer;

    public CustomButton(Game game, int levelIndex, LevelObserver observer) : base(game, levelIndex.ToString(), "") {
        _levelIndex = levelIndex;
        _observer = observer;
        AddObserver(observer);
    }

    public override void OnClick() {
        _observer.OnNotify(_levelIndex);
    }
}

public class SelectLevelLevel : Level {
    private float _inputDelay = 0.2f;
    private float _timeSinceLastInput = 0f;
    private UIHorizontalList _levelRow1, _levelRow2;
    private UIVerticalList _levelColumn;
    private int _levelSelected = 0;
    private int _lastLevelSelected = 0;
    private int _rowSelected = 0;
    private KeyboardState _lastKbState;
    LevelObserver _levelObserver;
    public SelectLevelLevel(Game game, int numberOfLevels) : base(game) {
        _levelType = LevelType.LevelSelect;
        _scene = new Scene();
        _uiScene = new Scene();

        _levelObserver = new LevelObserver(game as ProjectTinr);

        _levelRow1 = new UIHorizontalList(game) {
            Visible = true
        };
        _levelRow2 = new UIHorizontalList(game) {
            Visible = true
        };

        for (int i = 0; i < numberOfLevels; i++) {
            // todo: if level is locked, create a locked button instead
            CustomButton button = new(game, i + 1, _levelObserver) {
                String = $"{i + 1}",
                Visible = true,
            };

            if (i < numberOfLevels / 2) {
                _levelRow1.Children.Add(button);
            }
            else {
                _levelRow2.Children.Add(button);
            }
        }

        _levelColumn = new UIVerticalList(game) {
            Children = [_levelRow1, _levelRow2],
            Position = new(0, 0)
        };

        _uiScene.Add(_levelColumn);
    }
    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        KeyboardState kbState = Keyboard.GetState();
        _timeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_timeSinceLastInput < _inputDelay) {
            return;
        }

        if (Released(kbState, Keys.Enter) || Released(kbState, Keys.Space)){
            ((_levelColumn.Children[_rowSelected] as ComplexUIElement).Children[_levelSelected] as CustomButton).OnClick();
            _timeSinceLastInput = 0f;
        }

        if (kbState.IsKeyDown(Keys.Up) || kbState.IsKeyDown(Keys.Down)){
            _rowSelected = (_rowSelected + 1) % 2;
            if (_rowSelected == 0) {
                foreach(SimpleUIElement s in _levelRow2.Children) {
                    s.Selected = false;
                }
            } else {
                foreach(SimpleUIElement s in _levelRow1.Children) {
                    s.Selected = false;
                }
            }
            _timeSinceLastInput = 0f;
        }

        if (kbState.IsKeyDown(Keys.Left)) {
            _levelSelected -= 1;
            if (_levelSelected < 0) {
                _levelSelected = CurrentRowCount - 1;
            }
            _timeSinceLastInput = 0f;
        }

        if (kbState.IsKeyDown(Keys.Right)) {
            _levelSelected += 1; 
            _levelSelected %= CurrentRowCount;
            _timeSinceLastInput = 0f;
        }

        UpdateSelected();

        _lastLevelSelected = _levelSelected;
        _lastKbState = kbState;
    }

    private int CurrentRowCount  {
        get => (_levelColumn.Children[_rowSelected] as ComplexUIElement).Children.Count;
    }

    private void UpdateSelected() {
        UIHorizontalList row = _levelColumn.Children[_rowSelected] as UIHorizontalList;        
        (row.Children[_lastLevelSelected] as SimpleUIElement).Selected = false;
        (row.Children[_levelSelected] as SimpleUIElement).Selected = true;
    }

    private bool Pressed(KeyboardState currrentKB, Keys key) {
        return currrentKB.IsKeyDown(key) && !_lastKbState.IsKeyDown(key);
    }
    private bool Released(KeyboardState currrentKB, Keys key) {
        return !currrentKB.IsKeyDown(key) && _lastKbState.IsKeyDown(key);
    }
}
