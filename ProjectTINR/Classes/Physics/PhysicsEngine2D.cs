using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics.Shapes;

namespace ProjectTINR.Classes.Physics;

public class PhysicsEngine2D(Game game, Level level) : GameObject(game) {
    private readonly Level _level = level;

    private readonly Dictionary<string, ICollisionShape> _shapes = [];
    private readonly Dictionary<string, IPhysicsObject> _objs = [];

    public override void Initialize() {
        base.Initialize();
    }

    public override void Update(GameTime gameTime) {
        HashSet<string> updatedObjects = [];
        foreach (GameObject obj in _level.Scene) {
            if (obj is not IPhysicsObject) continue;
            IPhysicsObject physicsObject = (IPhysicsObject)obj;

            if (!_shapes.ContainsKey(obj.Name)) {
                ICollisionShape shape = CollisionShapeFactory.MakeShape(physicsObject.CollisionType);
                // Why don't we add these as Components? They're not animated and simply follow the sprite's position, so 
                // this wouldn't really make sense to do, and for debug drawing they're going to be a color not a sprite
                // What if these shapes had to affect the scene? Like removing themselves after being picked up?
                // check if they have a ISceneManipulator component, then pass a reference to the level?
                if (obj is Player) {
                    // PlayerCollisionShape playerShape = shape as PlayerCollisionShape;
                    // shape = new PlayerCollisionShape();
                    Rectangle rectangle = new(0, 0, 50, 100);
                    ((RectCollisionShape)shape).Rectangle = rectangle;
                    // shape = playerShape;
                    // playerShape.Rectangle.Width = 30;
                    // playerShape.Height = 50;
                }
                if (obj is Floor) {
                    RectCollisionShape floorShape = shape as RectCollisionShape;
                    Rectangle rectangle = new(0, 0, 400, 20);
                    floorShape.Rectangle = rectangle;
                    shape = floorShape;
                }
                _shapes.Add(obj.Name, shape);
                _objs.Add(obj.Name, physicsObject);
                Console.WriteLine("Added new shape");
            }
            updatedObjects.Add(obj.Name);
            // Console.WriteLine(updatedObjects.Count + " updated objects in physics engine.");

            if (_shapes[obj.Name].ShouldSimulate) {
                _shapes[obj.Name].Position = physicsObject.Position;

                if (_shapes[obj.Name] is IMoveComponent move) {
                    move.Velocity = physicsObject.Velocity;
                }

                // Update gravity 
                // Console.WriteLine("Applied gravity for: " + obj.Name);
                Vector2 objVeloc = ((IMoveComponent)obj).Velocity;
                objVeloc.Y += (int)(10000.0 * (float)gameTime.ElapsedGameTime.TotalSeconds);
                ((IMoveComponent)obj).Velocity = objVeloc;

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
        // Console.WriteLine(deleteMe.Count + " shapes to delete in physics engine.");
        foreach (string name in deleteMe) {
            _shapes.Remove(name);
        }
        // Console.WriteLine("Tuki sem jaaaaaa.");
        string[] keys = [.. _shapes.Keys];
        int length = keys.Length;
        for (int i = 0; i < length - 1; i++) {
            for (int j = i + 1; j < length; j++) {
                // Check collision - use another class to get algorightms 
                // If they're overlapping, move them back to the point of collision (touching)
                // this can also be done in the algorithms class 
                // Depending on static or non-static we may want to simulate the bounce or not 
                RectCollisionShape shapeA = (RectCollisionShape)_shapes[keys[i]];
                RectCollisionShape shapeB = (RectCollisionShape)_shapes[keys[j]];
                // float dist = Vector2.DistanceSquared(shapeA.Position, shapeB.Position);
                // if (dist >= 10000f) continue; // too far for collision anyway 

                // Use CollisionAlgorithms class to check for collision based on shape types
                bool isColliding = CollisionAlgorithms.CheckCollision(shapeA, shapeB);

                if (!isColliding) {
                    Console.WriteLine("No Collision between " + keys[i] + " and " + keys[j]);
                    continue;
                }
                Console.WriteLine("Collision detected between " + keys[i] + " and " + keys[j]);

                // Pa bomo vidl če to dela
                if (shapeA.ShouldSimulate) {
                    _objs[keys[i]].Position = shapeA.Position;
                    ((IMoveComponent)_objs[keys[i]]).Velocity = shapeA.Velocity;
                }
                if (shapeB.ShouldSimulate) {
                    _objs[keys[j]].Position = shapeB.Position;
                    ((IMoveComponent)_objs[keys[j]]).Velocity = shapeB.Velocity;
                }

                // Notify both shapes of the collision and resolve if both agree
                if (true /* shapeA.OnCollision(shapeB) && shapeB.OnCollision(shapeA)*/) {
                    Console.WriteLine("Resolving Collision between " + keys[i] + " and " + keys[j]);
                    CollisionAlgorithms.ResolveCollision(shapeA, shapeB);
                }
                // _level.Scene.GetObjectByName(keys[i]).Position = shapeA.Position;
                // _level.Scene.GetObjectByName(keys[j]).Position = shapeB.Position;
                // _objs[keys[j]].Position = shapeB.Position;

                
            }
        }
        base.Update(gameTime);
    }

    private void CollisionCheck(GameTime gameTime) {

    }
}
