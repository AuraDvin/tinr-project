using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjectTINR.Classes;
using ProjectTINR.Classes.Graphics;
using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics;

namespace ProjectTINR;

public class ProjectTinr : Game {
    private readonly GraphicsDeviceManager _graphics;
    private GameRenderer2D _gameRenderer;
    private UiRenderer2D _uiRenderer2D;
    private PhysicsEngine2D _physicsEngine;
    private GameInput _gameInput;
    private DebugPhysicsRender2D _debugRender2D;
    private Level _level;

    private Microsoft.Xna.Framework.Input.KeyboardState _prevKeyboardState;

    public ProjectTinr() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        SwitchLevel(LevelType.MainLevel);

        base.Initialize();
    }

    private void SwitchLevel(LevelType newLevelType) {
        // Remove current components if any
        if (_level != null) Components.Remove(_level);
        if (_gameInput != null) Components.Remove(_gameInput);
        if (_gameRenderer != null) Components.Remove(_gameRenderer);
        if (_physicsEngine != null) Components.Remove(_physicsEngine);
        if (_debugRender2D != null) Components.Remove(_debugRender2D);
        if (_uiRenderer2D != null) Components.Remove(_uiRenderer2D);

        _level = LevelFactory.CreateLevel(this, newLevelType);
        _gameInput = new GameInput(this, _level);
        _gameRenderer = new GameRenderer2D(this, _level);
        _physicsEngine = new PhysicsEngine2D(this, _level);
        _debugRender2D = new DebugPhysicsRender2D(this, _physicsEngine);
        _uiRenderer2D = new UiRenderer2D(this, _level);

        Components.Add(_level);
        Components.Add(_gameInput);
        Components.Add(_gameRenderer);
        Components.Add(_physicsEngine);
        Components.Add(_debugRender2D);
        Components.Add(_uiRenderer2D);
    }

    protected override void LoadContent() {
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime) {
        var kb = Keyboard.GetState();

        // Toggle into Settings with F1 (edge triggered)
        if (kb.IsKeyDown(Keys.F1) && !_prevKeyboardState.IsKeyDown(Keys.F1)) {
            if (_level.Type == LevelType.Settings) {
                SwitchLevel(LevelType.MainLevel);

            } else {
                // serialize level data / state 

                // switch to settings
                SwitchLevel(LevelType.Settings);
            }
        }

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            kb.IsKeyDown(Keys.Escape))
            Exit();

        _prevKeyboardState = kb;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        base.Draw(gameTime);
    }
}