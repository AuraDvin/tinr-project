using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.NPCs;
using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics.Shapes;
namespace ProjectTINR.Classes.Physics;

public class PhysicsEngine2D(Game game, Level level) : GameObject(game) {
    private readonly Level _level = level;
    public readonly Dictionary<string, ICollisionShape> _shapes = [];
    public readonly Dictionary<string, IStaticPhysicsObject> _objs = [];

    public override void Initialize() {
        base.Initialize();
    }

    public override void Update(GameTime gameTime) {
        HashSet<string> updatedObjects = [];
        foreach (GameObject obj in _level.Scene) {
            if (obj is not IStaticPhysicsObject staticPhysicsObject) continue;
            ICollisionShape shape;
            // Why don't we add these as Components? They're not animated and simply follow the sprite's position, so 
            // this wouldn't really make sense to do, and for debug drawing they're going to be a color not a sprite
            // What if these shapes had to affect the scene? Like removing themselves after being picked up?
            // check if they have a ISceneManipulator component, then pass a reference to the level?
            if (!_shapes.TryGetValue(obj.Name, out ICollisionShape value)) {
                shape = CollisionShapeFactory.MakeShape(staticPhysicsObject);
                if (shape is ISceneManipulator ss) {
                    ss.Scene = _level.Scene;
                }
                _shapes.Add(obj.Name, shape);
                _objs.Add(obj.Name, staticPhysicsObject);
            }
            else {
                shape = value;
            }
            // Console.WriteLine($"{obj.Name} => {shape}, {shape.Position}");
            shape.BeginFrame();
            updatedObjects.Add(obj.Name);
            if (shape.ShouldSimulate) {
                // shape.Position = staticPhysicsObject.Position;
                if (shape is not IMoveComponent) {
                    continue;
                }

                IMoveComponent shapeMoveComponent = (IMoveComponent)shape;

                if (staticPhysicsObject is not IMoveComponent) {
                    continue;
                }

                IPhysicsObject physicsObject = (IPhysicsObject)staticPhysicsObject;

                if (shape is IUpdatableGameComponent updatableGameComponent) {
                    updatableGameComponent.Update(gameTime);
                }

                Vector2 objVeloc = physicsObject.Velocity;
                // if (objVeloc.LengthSquared() > 1000000f) {
                //     objVeloc.Normalize();
                //     objVeloc *= 1000.0f;
                // }
                if (Math.Abs(objVeloc.X) <= 1f) {
                    objVeloc.X = 0f;
                }
                if (Math.Abs(objVeloc.Y) <= 1f) {
                    objVeloc.Y = 0f;
                }

                shapeMoveComponent.Velocity = objVeloc;
                Vector2 oldPosition = physicsObject.Position;
                Vector2 deltaPosition = shapeMoveComponent.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 finalPosition = oldPosition + deltaPosition;
                physicsObject.Position = finalPosition;

                Console.WriteLine($"[PhysicsEngine2D] Object: {obj.Name} Velocity: {physicsObject.Velocity} startPos: {oldPosition} Position delta: {deltaPosition} Final Pos: {finalPosition}");

                _shapes[obj.Name] = shape;
            }
        }

        {
            HashSet<string> deleteMe = [];
            foreach (var key in _shapes.Keys.Where(key => !updatedObjects.Contains(key))) {
                _ = deleteMe.Add(key);
            }
            foreach (string key in deleteMe) {
                _ = _shapes.Remove(key);
            }
        }

        CheckCollisions();
        base.Update(gameTime);
    }

    /// Check collisions with CollisionAlgorithms
    /// If they're overlapping, move them back to the point of collision (touching)
    /// If they agree to be simulated, ResolveCollision is called 
    private void CheckCollisions() {
     string[] keys = [.. _shapes.Keys];
        int length = keys.Length;
        for (int i = 0; i < length - 1; i++) {
            for (int j = i + 1; j < length; j++) {
                var shapeA = _shapes[keys[i]];
                var shapeB = _shapes[keys[j]];
                bool isColliding = CollisionAlgorithms.CheckCollision(shapeA, shapeB);
                if (!isColliding) {
                    continue;
                }
                // Notify both shapes of the collision and resolve if both agree
                bool agreeShapeA = shapeA.OnCollision(shapeB);
                bool agreeShapeB = shapeB.OnCollision(shapeA);
                // Resolve collision if both shapes agree
                if (agreeShapeA && agreeShapeB) {
                    // Console.WriteLine("Resolving Collision between " + keys[i] + " and " + keys[j]);
                    CollisionAlgorithms.ResolveCollision(shapeA, shapeB);
                }
            }
        }
    }
    
}
