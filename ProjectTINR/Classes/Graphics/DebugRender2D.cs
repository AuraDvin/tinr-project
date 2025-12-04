using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR.Classes;
using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics;
using ProjectTINR.Classes.Physics.Shapes;

namespace ProjectTINR.Classes.Graphics;

public class DebugRender2D : DrawableGameComponent {
    private readonly Level _level;
    private readonly PhysicsEngine2D _physicsEngine;
    private readonly SpriteBatch _spriteBatch;
    private Texture2D _whitePixel;

    public DebugRender2D(Game game, Level level, PhysicsEngine2D physicsEngine) : base(game) {
        _level = level;
        _physicsEngine = physicsEngine;
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
    }

    protected override void LoadContent() {
        // Create a 1x1 white texture for drawing rectangles
        _whitePixel = new Texture2D(GraphicsDevice, 1, 1);
        _whitePixel.SetData(new[] { Color.White });
        base.LoadContent();
    }

    public override void Draw(GameTime gameTime) {
        _spriteBatch.Begin();

        // Draw all collision shapes from the physics engine
        foreach (GameObject obj in _level.Scene) {
            if (obj is not IPhysicsObject) {
                continue;
            }

            IPhysicsObject physicsObject = (IPhysicsObject)obj;
            // Access the collision shape from the physics engine
            // We'll draw based on the collision type and position
            if (physicsObject.CollisionType == CollisionShapeType.Rectangle) {
                // Create a temporary rectangle at the object's position
                // Assuming standard sizes for now
                Rectangle rect = GetCollisionRectangle(obj);
                DrawRectangle(rect, Color.Blue);
            }
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private Rectangle GetCollisionRectangle(GameObject obj) {
        if (obj is Player) {
            Vector2 pos = ((IPositionComponent)obj).Position;
            return new Rectangle((int)(pos.X), (int)(pos.Y), 50, 100);
        }
        if (obj is Floor) {
            Vector2 pos = ((IPositionComponent)obj).Position;
            return new Rectangle((int)(pos.X), (int)(pos.Y), 400, 20);
        }
        // Default fallback
        Vector2 defaultPos = ((IPositionComponent)obj).Position;
        return new Rectangle((int)(defaultPos.X - 25), (int)(defaultPos.Y - 25), 50, 50);
    }

    private void DrawRectangle(Rectangle rect, Color color) {
        // Draw top line
        _spriteBatch.Draw(_whitePixel, new Rectangle(rect.X, rect.Y, rect.Width, 2), color);
        // Draw bottom line
        _spriteBatch.Draw(_whitePixel, new Rectangle(rect.X, rect.Y + rect.Height - 2, rect.Width, 2), color);
        // Draw left line
        _spriteBatch.Draw(_whitePixel, new Rectangle(rect.X, rect.Y, 2, rect.Height), color);
        // Draw right line
        _spriteBatch.Draw(_whitePixel, new Rectangle(rect.X + rect.Width - 2, rect.Y, 2, rect.Height), color);
    }

    protected override void Dispose(bool disposing) {
        _whitePixel?.Dispose();
        _spriteBatch?.Dispose();
        base.Dispose(disposing);
    }
}

