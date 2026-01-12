using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ProjectTINR.Classes.ObjectsComponents;

namespace ProjectTINR.Classes.UI;

public class UIHorizontalList : GameObject, ComplexUIElement {
    public UIHorizontalList(Game game) : base(game) {
    }

    public Scene Children { get; set; } = [];
    public bool Visible { get; set; } = true;
    public float Spacing { get; set; } = 40;
    public bool HasString => String.Length > 0;
    public bool HasTexture => TextureName.Length > 0;
    public string String { get; set; } = "";
    public string TextureName { get; set; } = "";
    public Vector2 TextPosition { get; set; } = Vector2.Zero;
    public Vector2 TexturePosition { get; set; } = Vector2.Zero;
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Rectangle TextureRect { get; set; } = new Rectangle(0, 0, 32, 32);

    // private List<UIButton> _buttons = new();
    // public List<UIButton> Buttons { get => _buttons; set => _buttons = value; }
    // private int _selectedIndex = 0;
    // private Vector2 _position;
    // private int _spacing = 40;
    // // private int _buttonWidth = 120;
    // // private int _buttonHeight = 50;
    // private bool _visible = false;
    // private KeyboardState _previousKeyboardState;
    // private Texture2D _cursorTexture;
    // // private Texture2D _buttonBackgroundTexture;
    // private SpriteFont _spriteFont;

    // public UIHorizontalList(Game game) : base(game) {
    //     _previousKeyboardState = Keyboard.GetState();
    // }

    // private Texture2D CreateCursorTexture(Rectangle rect) {

    //     Texture2D texture = new Texture2D(Game.GraphicsDevice, rect.Width, rect.Height);
    //     Color[] data = new Color[rect.Width * rect.Height];

    //     // Create a simple arrow cursor shape
    //     for (int i = 0; i < data.Length; i++) {
    //         data[i] = Color.Transparent;
    //         if (i % rect.Width == 0 || i % rect.Width == rect.Width - 1 || i / rect.Width == 0 || i / rect.Width == rect.Height - 1) {
    //             data[i] = Color.White; // Border
    //         }
    //     }

    //     // // Draw arrow pointing up
    //     // for (int x = 10; x < 20; x++) {
    //     //     data[10 * 30 + x] = Color.Yellow;
    //     // }
    //     // for (int y = 11; y < 20; y++) {
    //     //     data[y * 30 + 8] = Color.Yellow;
    //     //     data[y * 30 + 21] = Color.Yellow;
    //     // }

    //     texture.SetData(data);
    //     return texture;
    // }

    // // private Texture2D CreateButtonBackgroundTexture() {

    // //     Texture2D texture = new Texture2D(Game.GraphicsDevice, _buttonWidth, _buttonHeight);
    // //     Color[] data = new Color[_buttonWidth * _buttonHeight];

    // //     // Fill with semi-transparent blue
    // //     for (int i = 0; i < data.Length; i++) {
    // //         data[i] = new Color(50, 100, 150, 200);
    // //     }

    // //     // Add border
    // //     for (int x = 0; x < _buttonWidth; x++) {
    // //         data[0 * _buttonWidth + x] = Color.White;
    // //         data[(_buttonHeight - 1) * _buttonWidth + x] = Color.White;
    // //     }
    // //     for (int y = 0; y < _buttonHeight; y++) {
    // //         data[y * _buttonWidth + 0] = Color.White;
    // //         data[y * _buttonWidth + (_buttonWidth - 1)] = Color.White;
    // //     }

    // //     texture.SetData(data);
    // //     return texture;
    // // }

    // private void UpdateButtonPositions() {
    //     // float totalWidth = _buttons.Count * (_buttonWidth + _spacing);
    //     // float startX = _position.X - (totalWidth / 2);

    //     // for (int i = 0; i < _buttons.Count; i++) {
    //     //     UIButton button = _buttons[i];
    //     //     button.TexturePosition = new Vector2(startX + i * (_buttonWidth + _spacing), _position.Y);

    //     //     // Center text on button
    //     //     if (_spriteFont != null && !string.IsNullOrEmpty(button.String)) {
    //     //         Vector2 textSize = _spriteFont.MeasureString(button.String);
    //     //         float textX = button.TexturePosition.X + (_buttonWidth - textSize.X) / 2;
    //     //         float textY = button.TexturePosition.Y + (_buttonHeight - textSize.Y) / 2;

    //     //         button.TextPosition = new Vector2(textX, textY);
    //     //     }
    //     // }
    // }

    public void Show() {
        Visible = true;
    }

    public void Hide() {
        Visible = false;
    }

    // public override void Update(GameTime gameTime) {
    //     if (!_visible) return;

    //     KeyboardState currentKeyboardState = Keyboard.GetState();

    //     if (currentKeyboardState.IsKeyDown(Keys.Left) && !_previousKeyboardState.IsKeyDown(Keys.Left)) {
    //         SelectPrevious();
    //     }

    //     if (currentKeyboardState.IsKeyDown(Keys.Right) && !_previousKeyboardState.IsKeyDown(Keys.Right)) {
    //         SelectNext();
    //     }

    //     if (currentKeyboardState.IsKeyDown(Keys.Enter) && !_previousKeyboardState.IsKeyDown(Keys.Enter)) {
    //         ActivateSelected();
    //     }

    //     _previousKeyboardState = currentKeyboardState;
    // }

    // public void Draw(SpriteBatch spriteBatch) {
    //     if (!_visible || _buttons.Count == 0) return;

    //     // Draw buttons with backgrounds
    //     for (int i = 0; i < _buttons.Count; i++) {
    //         UIButton button = _buttons[i];
    //         Vector2 buttonPos = button.TexturePosition;

    //         // Draw button background
    //         if (button.Texture != null) {
    //             spriteBatch.Draw(
    //                 button.Texture,
    //                 button.TexturePosition,
    //                 Color.White);
    //         }

    //         // Draw button text
    //         if (!string.IsNullOrEmpty(button.String) && _spriteFont != null) {
    //             spriteBatch.DrawString(
    //                 _spriteFont,
    //                 button.String,
    //                 button.TextPosition,
    //                 Color.White);
    //         }
    //     }

    //     // Draw cursor on selected button
    //     if (!Enabled) return; 

    //     if (_selectedIndex >= 0 && _selectedIndex < _buttons.Count) {
    //         // Vector2 selectedButtonPos = _buttons[_selectedIndex].TexturePosition == null ? _buttons[_selectedIndex].TextPosition : _buttons[_selectedIndex].TexturePosition;
    //         Vector2 selectedButtonPos;
    //         if (_buttons[_selectedIndex].Texture != null) {
    //             selectedButtonPos = _buttons[_selectedIndex].TexturePosition;

    //         } else {
    //             selectedButtonPos = _buttons[_selectedIndex].TextPosition;
    //         }

    //         Vector2 cursorPos = new Vector2(
    //             selectedButtonPos.X - 40,
    //             selectedButtonPos.Y - 40);
    //         Rectangle cursorRect = new Rectangle((int)cursorPos.X, (int)cursorPos.Y, _buttons[_selectedIndex].Texture.Width + 80, _buttons[_selectedIndex].Texture.Height+ 80);

    //         _cursorTexture = CreateCursorTexture(cursorRect);

    //         spriteBatch.Draw(
    //             _cursorTexture,
    //             cursorPos,
    //             Color.White
    //         );
    //     }
    // }

    // public string String { get; set; } = "";
    // public Texture2D Texture { get; set; }
    // public Vector2 TextPosition { get; set; }
    // public Vector2 TexturePosition { get; set; }
    // public bool Visible {
    //     get => _visible;
    //     set => _visible = value;
    // }
    // public new bool Enabled { get; set; } = true;
    // public Vector2 Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    // public Scene Children { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    // public bool HasString => throw new NotImplementedException();

    // public bool HasTexture => throw new NotImplementedException();

    // public string TextureName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}
