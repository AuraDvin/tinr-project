using System;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.Graphics;

public class Camera2D {
    // Camera world position (top left corner)
    public Vector2 Position { get; set; } = Vector2.Zero;
    public float Zoom { get; set; } = 1f;
    public Camera2D() {
    }
    public Matrix GetViewMatrix() {
        return Matrix.CreateTranslation(new Vector3(-Position, 0f)) *
               Matrix.CreateScale(Zoom, Zoom, 1f);
    }
}
