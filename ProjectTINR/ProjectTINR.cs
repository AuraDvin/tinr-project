using ProjectTINR.Classes;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProjectTINR;
using TINR.Classes.Graphics;

namespace TINR;

public class ProjectTinr : Game {
    private readonly GraphicsDeviceManager _graphics;
    private GameRenderer _gameRenderer;

    private Level _level; 

    public ProjectTinr() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        _level = LevelMaker.CreateLevel(this, LevelType.StartMenu);
        Components.Add(_level);
        _gameRenderer = new GameRenderer(this, _level);
        Components.Add(_gameRenderer);
        base.Initialize();
    }

    protected override void LoadContent() {
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        // _level.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        // Console.WriteLine("Drawing game.");
        // _gameRenderer.Draw(gameTime);
        base.Draw(gameTime);
    }
}