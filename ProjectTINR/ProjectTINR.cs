using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ProjectTINR.Classes;
using ProjectTINR.Classes.Graphics;
using ProjectTINR.Classes.Levels;
using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics;
using ProjectTINR.Classes.Sound;

namespace ProjectTINR;

public class ProjectTinr : Game {
    private GameRenderer2D _gameRenderer;
    private UiRenderer2D _uiRenderer2D;
    private PhysicsEngine2D _physicsEngine;
    private GameInput _gameInput;
    private DebugPhysicsRender2D _debugRender2D;
    private GameSoundController _gameSoundController;
    private Level _level;
    private KeyboardState _prevKeyboardState;

    // Stack of previously pushed levels and their associated components
    private readonly Stack<LevelStackEntry> _levelStack = new();

    public ProjectTinr() : base() {
        new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        SwitchLevel(LevelType.StartMenu);
        GameSettings.Initialize();
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1 / 60f);

        base.Initialize();
    }


    public void SwitchLevel(LevelType newLevelType) {
        if (_level is MainLevel) {
            _level.Serialize();
        }

        // If we currently have a level, push it and its peripherals onto the stack and disable them
        if (_level != null) {
            var entry = new LevelStackEntry {
                Level = _level,
                GameInput = _gameInput,
                GameRenderer = _gameRenderer,
                Physics = _physicsEngine,
                DebugRender = _debugRender2D,
                UiRenderer = _uiRenderer2D,
                SoundController = _gameSoundController
            };

            _levelStack.Push(entry);

            // Disable so they are not updated or drawn while underneath
            entry.Level.Enabled = false;
            if (entry.GameInput != null) { entry.GameInput.Enabled = false; entry.GameInput.RemoveControllers(); }
            if (entry.GameRenderer != null) { entry.GameRenderer.Enabled = false; entry.GameRenderer.Visible = false; }
            if (entry.Physics != null) entry.Physics.Enabled = false;
            if (entry.DebugRender != null) entry.DebugRender.Enabled = false;
            if (entry.UiRenderer != null) entry.UiRenderer.Enabled = false;
            if (entry.SoundController != null) entry.SoundController.Enabled = false;

            foreach (GameObject thing in entry.Level.Scene) {
                thing.Enabled = false;
            }
            foreach (GameObject thing in entry.Level.UIScene) {
                thing.Enabled = false;
            }
        }

        // Create a fresh level on top
        _level = LevelFactory.CreateLevel(this, newLevelType);
        _gameInput = new GameInput(this, _level);
        _gameRenderer = new GameRenderer2D(this, _level);
        _physicsEngine = new PhysicsEngine2D(this, _level);
        _debugRender2D = new DebugPhysicsRender2D(this, _level);
        _uiRenderer2D = new UiRenderer2D(this, _level);
        _gameSoundController = new GameSoundController(this, _level);

        Components.Add(_level);
        Components.Add(_gameInput);
        Components.Add(_gameRenderer);
        Components.Add(_physicsEngine);
        Components.Add(_debugRender2D);
        Components.Add(_uiRenderer2D);
        Components.Add(_gameSoundController);
    }

    protected override void LoadContent() {
        base.LoadContent();
        GameSettings.SpriteFont = Content.Load<SpriteFont>("gameFont");
    }

    protected override void Update(GameTime gameTime) {
        var kb = Keyboard.GetState();

        // Toggle into Settings with F1 (edge triggered)
        if (kb.IsKeyDown(Keys.F1) && !_prevKeyboardState.IsKeyDown(Keys.F1)) {
            if (_level.Type == LevelType.Settings) {
                // if we're in settings, go back to the previous level on the stack
                ToPrevLevel();

            }
            else {
                // serialize level data / state 

                // switch to settings (push current level)
                SwitchLevel(LevelType.Settings);
            }
        }

        // Resert level with R 
        if (kb.IsKeyUp(Keys.R) && _prevKeyboardState.IsKeyDown(Keys.R)) {
            _level.Reset();
        }

        // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
        //     kb.IsKeyDown(Keys.Escape))
        //     Exit();

        _prevKeyboardState = kb;

        base.Update(gameTime);
    }

    // Switch back to the previous level on the stack, restoring its components
    public void ToPrevLevel() {
        if (_level is MainLevel) {
            _level.Serialize();
        }

        // Remove current top-level components
        if (_level != null) {
            Components.Remove(_level);
            foreach (GameObject thing in _level.Scene) {
                Components.Remove(thing);
            }
            if (_gameInput != null) {
                Components.Remove(_gameInput);
                _gameInput.RemoveControllers();
            }
            if (_gameRenderer != null) Components.Remove(_gameRenderer);
            if (_physicsEngine != null) Components.Remove(_physicsEngine);
            if (_debugRender2D != null) Components.Remove(_debugRender2D);
            if (_uiRenderer2D != null) Components.Remove(_uiRenderer2D);
            if (_gameSoundController != null) Components.Remove(_gameSoundController);
        }

        if (_levelStack.Count == 0) {
            // Nothing to go back to; create a default start menu
            _level = LevelFactory.CreateLevel(this, LevelType.StartMenu);
            _gameInput = new GameInput(this, _level);
            _gameRenderer = new GameRenderer2D(this, _level);
            _physicsEngine = new PhysicsEngine2D(this, _level);
            _debugRender2D = new DebugPhysicsRender2D(this, _level);
            _uiRenderer2D = new UiRenderer2D(this, _level);
            _gameSoundController = new GameSoundController(this, _level);

            Components.Add(_level);
            Components.Add(_gameInput);
            Components.Add(_gameRenderer);
            Components.Add(_physicsEngine);
            Components.Add(_debugRender2D);
            Components.Add(_uiRenderer2D);
            Components.Add(_gameSoundController);

            return;
        }

        var entry = _levelStack.Pop();

        // Restore components
        _level = entry.Level;
        _gameInput = entry.GameInput;
        _gameRenderer = entry.GameRenderer;
        _physicsEngine = entry.Physics;
        _debugRender2D = entry.DebugRender;
        _uiRenderer2D = entry.UiRenderer;
        _gameSoundController = entry.SoundController;

        // Re-enable and ensure they are present in the Components collection
        _level.Enabled = true;
        if (_gameInput != null) { _gameInput.Enabled = true; _gameInput.AddControllers(); }
        if (_gameRenderer != null) { _gameRenderer.Enabled = true; _gameRenderer.Visible = true; }
        if (_physicsEngine != null) _physicsEngine.Enabled = true;
        if (_debugRender2D != null) _debugRender2D.Enabled = true;
        if (_uiRenderer2D != null) _uiRenderer2D.Enabled = true;
        if (_gameSoundController != null) _gameSoundController.Enabled = true;

        foreach (GameObject thing in _level.Scene) {
            thing.Enabled = true;
            if (!Components.Contains(thing)) Components.Add(thing);
        }
        foreach (GameObject thing in _level.UIScene) {
            thing.Enabled = true;
            if (!Components.Contains(thing)) Components.Add(thing);
        }

        if (!Components.Contains(_level)) Components.Add(_level);
        if (_gameInput != null && !Components.Contains(_gameInput)) Components.Add(_gameInput);
        if (_gameRenderer != null && !Components.Contains(_gameRenderer)) Components.Add(_gameRenderer);
        if (_physicsEngine != null && !Components.Contains(_physicsEngine)) Components.Add(_physicsEngine);
        if (_debugRender2D != null && !Components.Contains(_debugRender2D)) Components.Add(_debugRender2D);
        if (_uiRenderer2D != null && !Components.Contains(_uiRenderer2D)) Components.Add(_uiRenderer2D);
        if (_gameSoundController != null && !Components.Contains(_gameSoundController)) Components.Add(_gameSoundController);

    }

    protected override void Draw(GameTime gameTime) {
        base.Draw(gameTime);
    }

    // Internal container for stacking a level and its related components
    private class LevelStackEntry {
        public Level Level { get; set; }
        public GameInput GameInput { get; set; }
        public GameRenderer2D GameRenderer { get; set; }
        public PhysicsEngine2D Physics { get; set; }
        public DebugPhysicsRender2D DebugRender { get; set; }
        public UiRenderer2D UiRenderer { get; set; }
        public GameSoundController SoundController { get; set; }
    }

    protected override void Dispose(bool disposing) {
        Console.WriteLine("Called dispose");
        base.Dispose(disposing);
        GameSettings.SaveSettings();
    }
}