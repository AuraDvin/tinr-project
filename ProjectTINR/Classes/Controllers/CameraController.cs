using System;
using System.Security.Cryptography;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Controllers;

public class CameraController(Game game) : GameObject(game), IController {
    public GameObject Owner { get; set; }

    public override void Initialize() {
    }

    public override void Update(GameTime gameTime) {
        Console.WriteLine("Updating CameraController.");
        ICameraComponent camera = Owner as ICameraComponent;
        if (Owner == null || camera == null) {
            Console.WriteLine("CameraController has no valid owner.");
            return;
        }
        KeyboardState kb = Keyboard.GetState();
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        float speed = 50f * dt;

        if (kb.IsKeyDown(Keys.W)) {
            camera.Position -= new Vector2(0, speed);
        }

        if (kb.IsKeyDown(Keys.A)) {
            camera.Position -= new Vector2(speed, 0);
        }

        if (kb.IsKeyDown(Keys.S)) {
            camera.Position -= new Vector2(0, -speed);
        }
        
        if (kb.IsKeyDown(Keys.D)) {
            camera.Position -= new Vector2(-speed, 0);
        }

        if (kb.IsKeyDown(Keys.Q)) {
            camera.Zoom += 0.5f * dt;
        }

        if (kb.IsKeyDown(Keys.E)) {
            camera.Zoom -= 0.5f * dt;
            if (camera.Zoom < 0.1f) {
                camera.Zoom = 0.1f;     
            }
        }

        Console.WriteLine($"Camera Position: {camera.Position}, Zoom: {camera.Zoom}, {camera}");
    }
}
