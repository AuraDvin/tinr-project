using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Controllers;

public class CameraController(Game game) : GameObject(game), IController, ISceneManipulator {
    public GameObject Owner { get; set; }
    public Scene Scene { get; set; }

    private float _smoothSpeed = 5f;
    private Vector2 _cameraOffset = new(0, 0);
    private float _deadZoneHeight = 0f;
    private readonly float _lookAheadDistance = 500f;
    private Vector2 _lookAheadTarget = Vector2.Zero;
    private readonly float _lookAheadSpeed = 3f;
    private Vector2 _screenCenter;
    private float _targetZoom = 1f;
    private readonly float _zoomSpeed = 2f;
    private readonly float _minZoom = 0.5f;
    private readonly float _maxZoom = 2f;
    private Player _player;
    private ICameraComponent _camera;
    private bool _override = false;

    public override void Initialize() {
        _screenCenter = new Vector2(
            Game.GraphicsDevice.Viewport.Width / 2f,
            Game.GraphicsDevice.Viewport.Height / 2f
        );
    }

    public override void Update(GameTime gameTime) {
        ICameraComponent camera = Owner as ICameraComponent;
        if (Owner == null || camera == null) {
            Console.WriteLine("Missing camera owner");
            return;
        }

        _camera = camera;

        if (_player == null) {
            FindPlayer();
            if (_player == null) return;
        }

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Vector2 targetPosition = CalculateTargetPosition();

        Vector2 smoothedPosition = Vector2.Lerp(
            camera.Position,
            targetPosition,
            _smoothSpeed * dt
        );

        // Ignore if we're moving the camera manually
        if (!_override) {
            camera.Position = smoothedPosition;
        }

        camera.Zoom = MathHelper.Lerp(camera.Zoom, _targetZoom, _zoomSpeed * dt);

        camera.Zoom = MathHelper.Clamp(camera.Zoom, _minZoom, _maxZoom);

        HandleManualControls(dt);
    }

    private void FindPlayer() {
        if (Scene != null) {
            _player = Scene.FindByType<Player>();
        }
    }

    private Vector2 CalculateTargetPosition() {
        if (_player == null) return _camera.Position;

        Vector2 playerPos = _player.Position;
        Vector2 playerVelocity = _player.Velocity;

        // See more in front of where you're looking
        if (_player.Direction == PlayerDirection.Right) {
            playerPos += new Vector2(194, 0);
        }

        // Had issues where you couldn't see the ground quick enough
        if (playerVelocity.Y > 0) {
            playerPos.Y += 194 * 3 / 4;
        }

        if (playerVelocity.LengthSquared() > 0.1f) {
            Vector2 lookDirection = playerVelocity;
            lookDirection.Normalize();
            _lookAheadTarget = lookDirection * _lookAheadDistance;
        }
        else {
            _lookAheadTarget = Vector2.Lerp(_lookAheadTarget, Vector2.Zero, _lookAheadSpeed * 0.016f);
        }

        Vector2 targetPos = playerPos + _lookAheadTarget - _screenCenter / _camera.Zoom;

        targetPos += _cameraOffset;

        // Should be removed; I don't think I was able to implement the wanted effect well 

        Vector2 cameraCenter = _camera.Position + _screenCenter / _camera.Zoom;
        Vector2 distanceFromCameraCenter = playerPos - cameraCenter;

        if (Math.Abs(distanceFromCameraCenter.X) > 0 ||
            Math.Abs(distanceFromCameraCenter.Y) > _deadZoneHeight) {

            if (distanceFromCameraCenter.X != 0) {
                targetPos.X = playerPos.X - _screenCenter.X / _camera.Zoom;
            }

            if (Math.Abs(distanceFromCameraCenter.Y) > _deadZoneHeight) {
                targetPos.Y = playerPos.Y - _screenCenter.Y / _camera.Zoom;
                targetPos.Y += (distanceFromCameraCenter.Y < 0 ? 1 : -1) * _deadZoneHeight;
            }

            return targetPos;
        }

        return _camera.Position;
    }


    private void HandleManualControls(float dt) {
        KeyboardState kb = Keyboard.GetState();
        float speed = 100f * dt;

        if (kb.IsKeyDown(Keys.LeftShift)) {
            _override = true;
            if (kb.IsKeyDown(Keys.Q)) {
                _targetZoom += 0.5f * dt;
            }
            if (kb.IsKeyDown(Keys.E)) {
                _targetZoom -= 0.5f * dt;
            }
            if (kb.IsKeyDown(Keys.W)) _camera.Position -= new Vector2(0, speed);
            if (kb.IsKeyDown(Keys.A)) _camera.Position -= new Vector2(speed, 0);
            if (kb.IsKeyDown(Keys.S)) _camera.Position -= new Vector2(0, -speed);
            if (kb.IsKeyDown(Keys.D)) _camera.Position -= new Vector2(-speed, 0);
        }
        else {
            // resets camera to how its supposed to be
            _override = false;
            _targetZoom = 1f; 
        }
    }
}