using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Physics.Shapes;

public class ProjectileCollisionShape : CircleCollisionShape, ISceneManipulator {
    Vector2 _position;
    Vector2 _velocity = new(10f, 0f);
    private float _lifeTime = 4f; 
    private float _sinceBorn = 0f;

    public ProjectileCollisionShape(Vector2 startingPosition, int direction) : base(false, 4f) {
        _position = startingPosition;
        if (direction == 0) direction = 1;
        _velocity *= Math.Sign(direction);
    }

    public override bool OnCollision(ICollisionShape other) {
        Scene.Remove(this);
        return false;
    }

    public override void Update(GameTime gameTime) {
        float dt =  (float)gameTime.ElapsedGameTime.TotalSeconds;
        _sinceBorn += dt;
        if (_sinceBorn >= _lifeTime) {
            Scene.Remove(this);
        }
        _position += _velocity * dt; 
        base.Update(gameTime);
    }
    public Scene Scene { get; set; }
}
