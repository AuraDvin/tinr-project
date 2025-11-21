using ProjectTINR.Classes.ObjectsComponents;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ProjectTINR.Classes;

public class PlayerController : IGameComponent, IUpdatableGameComponent {

    protected bool _isMovingLeft = false;
    protected bool _isMovingRight = false;

    public bool IsMovingLeft {
        get { return _isMovingLeft; }
    }
    public bool IsMovingRight {
        get { return _isMovingRight; }
    }

    public void Initialize() {
    }

    public void Update(GameTime gameTime) {
        if (Keyboard.GetState().IsKeyDown(Keys.Left)) {
            _isMovingLeft = true;
        } else {
            _isMovingLeft = false;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Right)) {
            _isMovingRight = true;
        } else {
            _isMovingRight = false;
        }
    }
}
