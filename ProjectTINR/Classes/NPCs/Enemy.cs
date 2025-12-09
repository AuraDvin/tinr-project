using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.NPCs;

public abstract class Enemy : GameObject, IPhysicsObject, IControlled, IDrawableGameComponent, ISceneManipulator {
    public virtual CollisionShapeType CollisionType { get; set; } = CollisionShapeType.StaticRectangle;
    public bool SeesPlayer = false;
    private float _seeingDistance = 450f;
    protected Enemy(Game game) : base(game) {
    }
    public Vector2 Velocity { get; set; }
    public Vector2 Position { get; set; }
    public virtual ControllerType ControllerType => ControllerType.AiController;

    public Scene Scene { get; set; } = null;

    public override void Update(GameTime gameTime) {
        try {
            Player player = Scene.FindByType<Player>();
            if (player == null) {
                Console.WriteLine("Player is null");
                return;
            }
            Vector2 vector2 = Position - player.Position;

            SeesPlayer = vector2.LengthSquared() <= _seeingDistance * _seeingDistance;
            Console.WriteLine($"Sees player? {SeesPlayer}");
            Console.WriteLine($"Sees player? {vector2.LengthSquared()}");
        }
        catch (System.Exception) {
            Console.WriteLine("Player is null");
            return;
        }

        base.Update(gameTime);
    }
}
