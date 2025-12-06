using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        foreach (ICollisionShape shape in _physicsEngine._shapes.Values) {
            if (shape is RectCollisionShape rectShape) {
                Rectangle rect = rectShape.Rectangle;
                DrawRectangle(rect, Color.Red);
            } else if (shape is CircleCollisionShape circle) {
                DrawCircle(circle, Color.Red);
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

    private void DrawCircle(CircleCollisionShape ccs, Color color) {
        int radius = (int)ccs.Radius;
        int outerRadius = radius * 2 + 2;
        Texture2D texture = new Texture2D(GraphicsDevice, outerRadius, outerRadius);

        Color[] data = new Color[outerRadius * outerRadius];

        // Colour the entire texture transparent first.
        for (int i = 0; i < data.Length; i++)
            data[i] = Color.Transparent;

        // Work out the minimum step necessary using trigonometry + sine approximation.
        double angleStep = 1f / radius;

        for (double angle = 0; angle < Math.PI * 2; angle += angleStep) {
            // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
            int x = (int)Math.Round(radius + radius * Math.Cos(angle));
            int y = (int)Math.Round(radius + radius * Math.Sin(angle));

            data[y * outerRadius + x + 1] = Color.White;
        }

        texture.SetData(data);
        _spriteBatch.Draw(texture, ccs.Position, color);
    }

    protected override void Dispose(bool disposing) {
        _whitePixel?.Dispose();
        _spriteBatch?.Dispose();
        base.Dispose(disposing);
    }
}

