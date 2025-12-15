using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics;

namespace ProjectTINR.Classes.NPCs;

public abstract class Enemy : GameObject, IPhysicsObject, IControlled, IDrawableGameComponent, ISceneManipulator {
    public virtual CollisionShapeType CollisionType { get; set; } = CollisionShapeType.StaticRectangle;
    public bool SeesPlayer = false;
    private float _seeingDistance = 450f;
    protected Enemy(Game game) : base(game) {
    }
    public virtual Vector2 Velocity { get; set; }
    public virtual Vector2 Position { get; set; }
    public virtual ControllerType ControllerType => ControllerType.AiController;

    public Scene Scene { get; set; } = null;

    public override void Update(GameTime gameTime) {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position += Velocity * dt;
        
        try {
            Player player = Scene.FindByType<Player>();
            if (player == null) {
                Console.WriteLine("Player is null");
                return;
            }
            Vector2 vector2 = Position - player.Position;

            SeesPlayer = vector2.LengthSquared() <= _seeingDistance * _seeingDistance;
            // Console.WriteLine($"Sees player? {SeesPlayer}");
            // Console.WriteLine($"Sees player? {vector2.LengthSquared()}");
        }
        catch (System.Exception) {
            Console.WriteLine("Player is null");
            return;
        }

        base.Update(gameTime);
    }
}
