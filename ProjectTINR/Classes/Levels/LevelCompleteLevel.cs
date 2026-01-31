using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProjectTINR.Classes.UI;
using ProjectTINR.Classes;

namespace ProjectTINR.Classes.Levels;

class NextLevelObserver : Observer {
    private Game _game;
    public NextLevelObserver(Game game) { _game = game; }
    public void Notify() {
        int next = GameSettings.LevelNum + 1;
        try {
            LevelDataManager.Instance.ReadData($"Content/levels/level{next}.json");
            GameSettings.LevelNum = next;
            if (_game is ProjectTinr p) p.SwitchLevel(LevelType.MainLevel);
        }
        catch (Exception) {
            LevelType fallback = LevelType.LevelSelect;
            if (next == 7) {
                fallback = LevelType.Credits;
            }
            if (_game is ProjectTinr p) p.SwitchLevel(fallback);
            else throw new Exception("could not switch to levelselect/credits");
        }
    }
    public void Notify(string message, object? args) { throw new NotImplementedException(); }
}

class BackToSelectObserver : Observer {
    private Game _game;
    public BackToSelectObserver(Game game) { _game = game; }
    public void Notify() {
        if (_game is ProjectTinr p) p.SwitchLevel(LevelType.LevelSelect);
    }
    public void Notify(string message, object? args) { throw new NotImplementedException(); }
}

public class LevelCompleteLevel : Level {
    private UiLabel _titleLabel;
    private UIButton _nextButton;
    private UIButton _backButton;
    private UIVerticalList _menuList;

    private KeyboardState _prevKb;
    private int _selected = 0;

    public LevelCompleteLevel(Game game) : base(game) {
        _scene = new Scene();
        _uiScene = new Scene();
    }

    public override void Initialize() {
        base.Initialize();
        _scene = [];
        _uiScene = [];

        _titleLabel = new(Game, $"Level {GameSettings.LevelNum} Complete!") {
            Position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - 200, Game.GraphicsDevice.Viewport.Height / 2 - 120)
        };

        _nextButton = new(Game, "Next Level", "") { Visible = true };
        _nextButton.AddObserver(new NextLevelObserver(Game));

        _backButton = new(Game, "Back to Level Select", "") { Visible = true };
        _backButton.AddObserver(new BackToSelectObserver(Game));

        _menuList = new(Game) {
            Children = [_nextButton, _backButton],
            Position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - 100, Game.GraphicsDevice.Viewport.Height / 2 - 40)
        };

        (_menuList.Children[0] as SimpleUIElement).Selected = true;

        _uiScene.Add(_titleLabel);
        _uiScene.Add(_menuList);

        _prevKb = Keyboard.GetState();
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);

        KeyboardState kb = Keyboard.GetState();

        if (kb.IsKeyUp(Keys.Up) && _prevKb.IsKeyDown(Keys.Up)) {
            (_menuList.Children[_selected] as SimpleUIElement).Selected = false;
            _selected -= 1;
            if (_selected < 0) _selected = _menuList.Children.Count - 1;
            (_menuList.Children[_selected] as SimpleUIElement).Selected = true;
        }

        if (kb.IsKeyUp(Keys.Down) && _prevKb.IsKeyDown(Keys.Down)) {
            (_menuList.Children[_selected] as SimpleUIElement).Selected = false;
            _selected += 1;
            _selected %= _menuList.Children.Count;
            (_menuList.Children[_selected] as SimpleUIElement).Selected = true;
        }

        if (kb.IsKeyUp(Keys.Enter) && _prevKb.IsKeyDown(Keys.Enter) ||
            kb.IsKeyUp(Keys.Space) && _prevKb.IsKeyDown(Keys.Space)) {
            (_menuList.Children[_selected] as UIButton).OnClick();
        }

        _prevKb = kb;
    }
}
