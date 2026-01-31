using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjectTINR.Classes.UI;

namespace ProjectTINR.Classes.Levels;

public class CreditsLevel : Level {
    private readonly UiLabel _title;
    private readonly UiLabel _subTitle;
    private readonly UIHorizontalList _menu;
    private readonly UIVerticalList _page;
    private float _inputDelay = 0.2f;
    private float _timeSinceLastInput = 0f;
    int selectedButton = 1;

    public CreditsLevel(Game game) : base(game) {
        _title = new(game, "Congrats on beating level 6");
        _subTitle = new(game, "Knives x Knight by AuraDvin");

        UIButton levelSelect = new(game, "Level Select");
        UIButton quit = new(game, "I'm done") {
            Selected = true
        };

        _menu = new UIHorizontalList(game) {
            Children = [levelSelect, quit]
        };

        _page = new UIVerticalList(game) {
            Children = [_title, _subTitle, _menu],
            Position = new(10, 0)
        };

        _uiScene.Add(_page);
    }
    KeyboardState _oldKb;
    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        
        _timeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_timeSinceLastInput < _inputDelay) {
            return;
        }

        KeyboardState kb = Keyboard.GetState();

        if (kb.IsKeyDown(Keys.Left) || kb.IsKeyDown(Keys.Right)) {
            (_menu.Children[selectedButton] as UIButton).Selected = false;
            selectedButton++;
            selectedButton %= _menu.Children.Count;
            _timeSinceLastInput = 0;
            (_menu.Children[selectedButton] as UIButton).Selected = true;
        }

        if ((kb.IsKeyUp(Keys.Enter) && _oldKb.IsKeyDown(Keys.Enter) )|| 
            (kb.IsKeyUp(Keys.Space) && _oldKb.IsKeyDown(Keys.Space) )) {
            if (selectedButton == 0) {
                (Game as ProjectTinr).SwitchLevelNoPush(LevelType.LevelSelect);
            } else {
                (Game as ProjectTinr).Exit();
            }
        }

        _oldKb = kb;    
    }
}
