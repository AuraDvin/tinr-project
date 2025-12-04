using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;

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
            if (obj is not IStaticPhysicsObject) continue;
            IStaticPhysicsObject staticPhysicsObject = (IStaticPhysicsObject)obj;
            ICollisionShape shape;
            if (!_shapes.ContainsKey(obj.Name)) {
                shape = CollisionShapeFactory.MakeShape(staticPhysicsObject.CollisionType);
                // Why don't we add these as Components? They're not animated and simply follow the sprite's position, so 
                // this wouldn't really make sense to do, and for debug drawing they're going to be a color not a sprite
                // What if these shapes had to affect the scene? Like removing themselves after being picked up?
                // check if they have a ISceneManipulator component, then pass a reference to the level?

                shape.Position = staticPhysicsObject.Position;
                _shapes.Add(obj.Name, shape);
                _objs.Add(obj.Name, staticPhysicsObject);
                Console.WriteLine("Added new shape");
            }
            else {
                shape = _shapes[obj.Name];
            }
            updatedObjects.Add(obj.Name);
            if (shape.ShouldSimulate) {
                shape.Position = staticPhysicsObject.Position;

                if (shape is not IMoveComponent) {
                    continue;
                }

                // ((IMoveComponent)_shapes[obj.Name]).Velocity = physicsObject.Velocity;
                // Console.WriteLine("Applied gravity for: " + obj.Name);
                if (staticPhysicsObject is not IMoveComponent) {
                    continue;
                }

                IPhysicsObject physicsObject = (IPhysicsObject)staticPhysicsObject;
                // Copy Velocity and apply gravity
                Vector2 objVeloc = physicsObject.Velocity;
                // if (_shapes[obj.Name] is PlayerCollisionShape playerCollisionShape && playerCollisionShape.OnFloor) {
                //     objVeloc.Y = Math.Min(0, objVeloc.Y);
                // }
                // else {
                //     // objVeloc.Y += (int)(8000.0 * (float)gameTime.ElapsedGameTime.TotalSeconds);
                // }
                objVeloc.Y += (int)(10000.0 * (float)gameTime.ElapsedGameTime.TotalSeconds);

                ((IMoveComponent)shape).Velocity = objVeloc;

                ((IPhysicsObject)_objs[obj.Name]).Velocity = objVeloc;
                _shapes[obj.Name] = shape;
            }
            // Add all other dynamic objects (like bullets, enemies, throwing knives)
            // Have to be logged dynanmically too, where they get a new position from the scene 
            // This means we can have all of the shapes for object types defined, but then we copy / Make a new shape for specific objects 
            // this should be done each update from strach, because if not, we have to log which didn't appear and delete them or have some broadcast...
        }

        HashSet<string> deleteMe = [];
        foreach (string key in _shapes.Keys) {
            if (!updatedObjects.Contains(key)) {
                deleteMe.Add(key);
            }
        }
        foreach (string name in deleteMe) {
            _shapes.Remove(name);
        }

        string[] keys = [.. _shapes.Keys];
        int length = keys.Length;
        for (int i = 0; i < length - 1; i++) {
            for (int j = i + 1; j < length; j++) {
                // Check collision - use another class to get algorightms 
                // If they're overlapping, move them back to the point of collision (touching)
                // this can also be done in the algorithms class 
                // Depending on static or non-static we may want to simulate the bounce or not 
                var shapeA = _shapes[keys[i]];
                var shapeB = _shapes[keys[j]];

                bool isColliding = CollisionAlgorithms.CheckCollision(shapeA, shapeB);
                if (!isColliding) {
                    continue;
                }

                _objs[keys[i]].Position = shapeA.Position;
                _objs[keys[j]].Position = shapeB.Position;

                if (_objs[keys[i]] is IPhysicsObject) {
                    ((IPhysicsObject)_objs[keys[i]]).Velocity = ((IMoveComponent)shapeA).Velocity;
                }

                if (_objs[keys[j]] is IPhysicsObject) {
                    ((IPhysicsObject)_objs[keys[j]]).Velocity = ((IMoveComponent)shapeB).Velocity;
                }

                // Notify both shapes of the collision and resolve if both agree
                bool oncA = shapeA.OnCollision(shapeB);
                bool oncB = shapeB.OnCollision(shapeA);

                _objs[keys[i]].Position = shapeA.Position;
                _objs[keys[j]].Position = shapeB.Position;

                if (_objs[keys[i]] is IPhysicsObject) {
                    ((IPhysicsObject)_objs[keys[i]]).Velocity = ((IMoveComponent)shapeA).Velocity;
                }
                if (_objs[keys[j]] is IPhysicsObject) {
                    ((IPhysicsObject)_objs[keys[j]]).Velocity = ((IMoveComponent)shapeB).Velocity;
                }

                if (oncA && oncB) {
                    Console.WriteLine("Resolving Collision between " + keys[i] + " and " + keys[j]);
                    CollisionAlgorithms.ResolveCollision(shapeA, shapeB);
                    _objs[keys[i]].Position = shapeA.Position;
                    _objs[keys[j]].Position = shapeB.Position;

                    if (_objs[keys[i]] is IPhysicsObject) {
                        ((IPhysicsObject)_objs[keys[i]]).Velocity = ((IMoveComponent)shapeA).Velocity;
                    }
                    if (_objs[keys[j]] is IPhysicsObject) {
                        ((IPhysicsObject)_objs[keys[j]]).Velocity = ((IMoveComponent)shapeB).Velocity;
                    }
                }
            }
        }
        base.Update(gameTime);
    }
}
