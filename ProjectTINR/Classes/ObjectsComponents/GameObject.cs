using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes;


public class GameObject : GameComponent {
    protected List<IGameComponent> _components = [];

    virtual protected string _prefix => "Object";

    private readonly string _name = "";

    public string Name { get => _prefix + _name; }

    public GameObject(Game game) : base(game) {
        _name = NameGeneratorSingleton.Instance.GetName(_prefix);
    }

    public override void Initialize() {
        base.Initialize();
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