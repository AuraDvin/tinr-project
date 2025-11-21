using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes;


public class GameObject : GameComponent {
    protected List<IGameComponent> _components = [];

    public GameObject(Game game) : base(game) {
    }

    public void Initialize() {
    }

    public void AddGameComponent(IGameComponent component) {
        _components.Add(component);
    }

    public T GetComponent<T>() where T : IGameComponent {
        foreach (var component in _components) {
            if (component is T typedComponent) {
                return typedComponent;
            }
        }
        return default(T);
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
    }

}