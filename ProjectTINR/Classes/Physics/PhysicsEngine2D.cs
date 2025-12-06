using System;
using System.Collections;
using System.Collections.Generic;
using System.Formats.Asn1;
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
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
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

                if (staticPhysicsObject is not IMoveComponent) {
                    continue;
                }

                IPhysicsObject physicsObject = (IPhysicsObject)staticPhysicsObject;
                Vector2 objVeloc = physicsObject.Velocity;

                if (physicsObject is Player player) {
                    PlayerCollisionShape playerShape = (PlayerCollisionShape)shape;
                    float playerAccel = 200f;
                    float playerJumpForce = 100000f;
                    float playerGravity = 10000f;
                    float playerFriction = 8f;
                    switch (player.State) {
                        case PlayerState.Idling:
                            objVeloc = Vector2.Lerp(objVeloc, new(0, objVeloc.Y), playerFriction * dt);
                            break;
                        case PlayerState.Moving:
                            if (player.Direction == PlayerDirection.Left) {
                                objVeloc.X += -playerAccel * dt;
                            }
                            else if (player.Direction == PlayerDirection.Right) {
                                objVeloc.X += playerAccel * dt;
                            }
                            break;
                        case PlayerState.Jumping:
                            if (shape is PlayerCollisionShape) {
                                if (playerShape.OnFloor) {
                                    Console.WriteLine("Player is jumping from floor.");
                                    objVeloc.Y = -playerJumpForce;
                                    objVeloc.X = player.Direction == PlayerDirection.Left ? -playerJumpForce : playerJumpForce;
                                    playerShape.OnFloor = false;
                                }
                                else {
                                    Console.WriteLine("Player is in the air, cannot jump again.");
                                }
                            }
                            break;
                        case PlayerState.Falling:
                            ;
                            break;
                        case PlayerState.Frozen:
                            objVeloc = Vector2.Zero;
                            break;
                        default:
                            break;
                    }

                    if (playerShape.OnFloor) objVeloc.Y = Math.Min(objVeloc.Y, 0);
                    else objVeloc.Y += playerGravity * dt;

                }

                if (objVeloc.LengthSquared() > 1000000f) {
                    objVeloc.Normalize();
                    objVeloc *= 1000.0f;
                }
                if (Math.Abs(objVeloc.X) <= 1f) {
                    objVeloc.X = 0f;
                }
                if (Math.Abs(objVeloc.Y) <= 1f) {
                    objVeloc.Y = 0f;
                }
                Console.WriteLine(((GameObject)physicsObject).Name + "'s velocity after physics update: " + objVeloc.ToString());

                ((IMoveComponent)shape).Velocity = objVeloc;
                ((IPhysicsObject)_objs[obj.Name]).Velocity = objVeloc;
                _shapes[obj.Name] = shape;
            }
            // Add all other dynamic objects (like bullets, enemies, throwing knives)
            // Have to be logged dynanmically too, where they get a new position from the scene 
            // This means we can have all of the shapes for object types defined, but then we copy / Make a new shape for specific objects 
            // this should be done each update from strach, because if not, we have to log which didn't appear and delete them or have some broadcast...
        }

        {
            HashSet<string> deleteMe = [];
            foreach (string key in _shapes.Keys) {
                if (!updatedObjects.Contains(key)) {
                    _ = deleteMe.Add(key);
                }
            }
            foreach (string key in deleteMe) {
                _ = _shapes.Remove(key);
            }
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
                foreach (var thing in new List<int>{ i, j }) {
                    if (_objs[keys[thing]] is IPhysicsObject object0) {
                        object0.Velocity = ((IMoveComponent)(thing == i ? shapeA : shapeB)).Velocity;
                    }
                }

                // Notify both shapes of the collision and resolve if both agree
                bool oncA = shapeA.OnCollision(shapeB);
                bool oncB = shapeB.OnCollision(shapeA);

                _objs[keys[i]].Position = shapeA.Position;
                _objs[keys[j]].Position = shapeB.Position;
                foreach (var thing in new List<int>{ i, j }) {
                    if (_objs[keys[thing]] is IPhysicsObject object0) {
                        object0.Velocity = ((IMoveComponent)(thing == i ? shapeA : shapeB)).Velocity;
                    }
                }

                if (oncA && oncB) {
                    Console.WriteLine("Resolving Collision between " + keys[i] + " and " + keys[j]);
                    CollisionAlgorithms.ResolveCollision(shapeA, shapeB);
                    _objs[keys[i]].Position = shapeA.Position;
                    _objs[keys[j]].Position = shapeB.Position;
                    foreach (var thing in new List<int>{ i, j }) {
                        if (_objs[keys[thing]] is IPhysicsObject object0) {
                            object0.Velocity = ((IMoveComponent)(thing == i ? shapeA : shapeB)).Velocity;
                        }
                    }
                }
            }
        }
        base.Update(gameTime);
    }
}
