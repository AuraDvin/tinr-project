using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics;

namespace ProjectTINR.Classes.Objects;

public class Floor : GameObject, IStaticPhysicsObject, IDrawableGameComponent {
    public Floor(Game game, Vector2 position, int width, int height) : base(game) {
        BoundingBox = new Rectangle((int)position.X, (int)position.Y, width, height);
        _position = position;

    }
    protected override string _prefix => "Floor";
    private Vector2 _position = new();
    public CollisionShapeType CollisionType { get => CollisionShapeType.FloorCollisionShape; set => throw new NotImplementedException(); }
    public Vector2 Position { get => _position; set { _rectangle.X = (int)value.X; _rectangle.Y = (int)value.Y; _position = value; } }
    private Rectangle _rectangle = new();
    public Rectangle BoundingBox {
        get => new Rectangle((int)Position.X ,(int)Position.Y, _rectangle.Width, _rectangle.Height); 
        set { _position.X = value.X; _position.Y = value.Y; _rectangle.Width = value.Width; _rectangle.Height = value.Height; } 
    }
    public override void Initialize() {
        base.Initialize();
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
    }
}
