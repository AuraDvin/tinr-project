using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.Controllers;
using ProjectTINR.Classes.NPCs;

namespace ProjectTINR.Classes.ObjectsComponents;

public class GameInput(Game game, Level level) : GameObject(game) {
    private readonly Level _level = level;
    private readonly Dictionary<string, IController> _controllers = [];
    public override void Initialize() {
        base.Initialize();
    }
    public override void Update(GameTime gameTime) {
        HashSet<string> updatedObjects = [];
        foreach (GameObject obj in _level.Scene) {
            if (obj is not IControlled) {
                continue;
            }

            IControlled controlledObject = (IControlled)obj;
            IController controller;

            if (!_controllers.ContainsKey(obj.Name)) {
                controller = ControllerFactory.CreateController(Game, controlledObject);
                if (controller is ISceneManipulator sceneManipulator) {
                    sceneManipulator.Scene = _level.Scene;
                }
                if (controller is EnemyController ec) {
                    ec.Owner = (StationaryEnemy)controlledObject;
                }
                _controllers.Add(obj.Name, controller);
                Game.Components.Add(controller);
                // Console.WriteLine($"Added controller for {obj.Name}");
            }

            updatedObjects.Add(obj.Name);
        }

        // Clean up removed objects
        var objectsToRemove = new List<string>();
        foreach (var controllerName in _controllers.Keys) {
            if (!updatedObjects.Contains(controllerName)) {
                objectsToRemove.Add(controllerName);
            }
        }

        foreach (var name in objectsToRemove) {
            Game.Components.Remove(_controllers[name]);
            _controllers.Remove(name);
            // Console.WriteLine($"Removed controller for {name}");
        }
    }
}
