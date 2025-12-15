using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.Controllers;

public interface IController : IUpdatableGameComponent {
    GameObject Owner { get; }
}
