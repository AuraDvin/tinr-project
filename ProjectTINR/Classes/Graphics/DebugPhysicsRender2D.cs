using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR.Classes;
using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics;
using ProjectTINR.Classes.Physics.Shapes;

namespace ProjectTINR.Classes.Graphics;

public class DebugPhysicsRender2D : DrawableGameComponent {
    private readonly PhysicsEngine2D _physicsEngine;
    private readonly SpriteBatch _spriteBatch;
    private Texture2D _whitePixel;

    public DebugPhysicsRender2D(Game game, PhysicsEngine2D physicsEngine) : base(game) {
        _physicsEngine = physicsEngine;
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
    }

    protected override void LoadContent() {
        // Create a 1x1 white texture for drawing rectangles
        _whitePixel = new Texture2D(GraphicsDevice, 1, 1);
        _whitePixel.SetData([Color.White]);
        base.LoadContent();
    }

    public override void Draw(GameTime gameTime) {
        _spriteBatch.Begin();

        foreach(ICollisionShape shape in _physicsEngine._shapes.Values) {
            if (shape is RectCollisionShape rectShape) {
                Rectangle rect = rectShape.Rectangle;
                DrawRectangle(rect, Color.Red);
            }
        }
        

        _spriteBatch.End();
        base.Draw(gameTime);
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

