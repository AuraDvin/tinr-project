using ProjectTINR.Classes;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProjectTINR;
using TINR.Classes.Graphics;
using ProjectTINR.Classes.Physics;
using ProjectTINR.Classes.Graphics;

namespace TINR;

public class ProjectTinr : Game {
    private readonly GraphicsDeviceManager _graphics;
    private GameRenderer2D _gameRenderer;
    private PhysicsEngine2D _physicsEngine;
    private DebugRender2D _debugRender2D;
    private Level _level; 

    public ProjectTinr() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        _level = LevelMaker.CreateLevel(this, LevelType.StartMenu);
        _gameRenderer = new GameRenderer2D(this, _level);
        _physicsEngine = new PhysicsEngine2D(this, _level); 
        _debugRender2D = new DebugRender2D(this, _level, _physicsEngine);

        Components.Add(_level);
        Components.Add(_gameRenderer);
        Components.Add(_physicsEngine);
        Components.Add(_debugRender2D);

        base.Initialize();
    }

    protected override void LoadContent() {
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        base.Draw(gameTime);
    }
}