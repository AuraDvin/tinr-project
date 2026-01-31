using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics;

namespace ProjectTINR.Classes.NPCs;

public abstract class Enemy(Game game) : GameObject(game), IPhysicsObject, IControlled, IDrawableGameComponent, ISceneManipulator {
    public virtual CollisionShapeType CollisionType { get; set; } = CollisionShapeType.StaticRectangle;
    public bool SeesPlayer = false;
    protected float _seeingDistance = 450f;
    protected float _health = 3;
    public float Health { get => _health; set => _health = value; }
    public virtual Vector2 Velocity { get; set; }
    public virtual Vector2 Position { get; set; }
    public virtual ControllerType ControllerType => ControllerType.AiController;
    public Scene Scene { get; set; } = null;

    public override void Update(GameTime gameTime) {
        if (Health <= 0) {
            Scene.Remove(this);
        }

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position += Velocity * dt;

        Player player = Scene.FindByType<Player>();
        if (player == null) {
            // Console.WriteLine("Player is null");
            return;
        }
        Vector2 vector2 = Position - player.Position;

        SeesPlayer = vector2.LengthSquared() <= _seeingDistance * _seeingDistance;

        base.Update(gameTime);
    }
}
