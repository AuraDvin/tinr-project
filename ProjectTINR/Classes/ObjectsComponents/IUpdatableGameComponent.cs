using Microsoft.Xna.Framework;

namespace ProjectTINR.Classes.ObjectsComponents;

public interface IUpdatableGameComponent : IGameComponent
{
    void Update(GameTime gameTime);
}
