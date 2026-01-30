using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.AccessControl;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.UI;

namespace ProjectTINR.Classes.Graphics;

public class UiRenderer2D(Game game, Level level) : GameRenderer2D(game, level)
{
    private readonly Level _level = level;
    private Dictionary<IUiDrawableComponent, Texture2D> _textures = [];
    private Dictionary<IUiDrawableComponent, string> _labels = [];
    private Dictionary<IUiDrawableComponent, Rectangle> _boundingBoxes = [];
    readonly SpriteFont _spriteFont = game.Content.Load<SpriteFont>("gameFont");

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _textures = [];
        _labels = [];
        _boundingBoxes = [];
        /* 
        Todo: 
            - Update any ui element that is updatable 
            - Handle all the labels and textures to draw
            - Update / Use bounding boxes to get positions of all the child of complex elements
        */

        foreach (GameObject obj in _level.UIScene)
        {
            // By default none of these should be updating but I might as well leave this here 
            obj.Update(gameTime);
            if (obj is ComplexUIElement complexUI)
            {
                if (!complexUI.Visible) continue;
                handleComplex(complexUI);
                CalculateBoundingBox(complexUI);
            }
            else if (obj is SimpleUIElement sie)
            {
                if (!sie.Visible) continue;
                handleSimple(sie);
                CalculateBoundingBox(sie);
            }
        }
    }

    public Rectangle CalculateBoundingBox(SimpleUIElement sie)
    {
        Vector2 position = sie.Position;
        float stringX = 0f;
        float stringY = 0f;

        if (sie.HasString)
        {
            Vector2 stringSize = GameSettings.SpriteFont.MeasureString(sie.String);
            stringX += sie.TextPosition.X + stringSize.X;
            stringY += sie.TextPosition.Y + stringSize.Y;
        }
        float textX = 0f, textY = 0f;
        if (sie.HasTexture)
        {
            textX += sie.TexturePosition.X + sie.TextureRect.Width;
            textY += sie.TexturePosition.Y + sie.TextureRect.Height;
        }
        Rectangle box = new Rectangle(
            (int)position.X,
            (int)position.Y,
            (int)Math.Max(stringX, textX),
            (int)Math.Max(stringY, textY)
        );
        _boundingBoxes.Add(sie, box);
        return box;
    }

    public Rectangle CalculateBoundingBox(ComplexUIElement cie)
    {
        Vector2 position = cie.Position;

        Rectangle[] boxes = new Rectangle[cie.Children.Count > 0 ? cie.Children.Count : 1];
        if (cie.Children.Count == 0) boxes[0] = new Rectangle(0, 0, 0, 0);

        for (int i = 0; i < cie.Children.Count; i++)
        {
            var child = cie.Children[i];
            switch (child)
            {
                case ComplexUIElement ciechild:
                    boxes[i] = CalculateBoundingBox(ciechild);
                    break;
                case SimpleUIElement siechild:
                    boxes[i] = CalculateBoundingBox(siechild);
                    break;
            }
            if (i == 0) continue;
            Rectangle prevBox = boxes[i - 1];
            switch (cie)
            {
                case UIVerticalList vl:
                    boxes[i].Y += (int)(prevBox.Y + prevBox.Height + vl.Spacing);
                    break;
                case UIHorizontalList hl:
                    boxes[i].X += (int)(prevBox.X + prevBox.Width + hl.Spacing);
                    break;
            }
            if (child is SimpleUIElement siee)
                _boundingBoxes[siee] = boxes[i];
            else if (child is ComplexUIElement ciee)
            {
                _boundingBoxes[ciee] = boxes[i];
            }
        }


        Rectangle box = new Rectangle(
            (int)position.X,
            (int)position.Y,
            boxes.Last().Right,
            boxes.Last().Bottom
        );

        _boundingBoxes.Add(cie, box);
        return box;
    }

    private void handleSimple(SimpleUIElement sie)
    {
        if (sie.HasString)
        {
            _labels.Add(sie, sie.String);
        }
        if (sie.HasTexture)
        {
            _textures.Add(sie, Game.Content.Load<Texture2D>(sie.TextureName));
        }
    }

    private void handleComplex(ComplexUIElement cie)
    {
        for (int i = 0; i < cie.Children.Count; i++)
        {
            switch (cie.Children[i])
            {
                case ComplexUIElement ccie:
                    handleComplex(ccie);
                    break;
                case SimpleUIElement sie:
                    handleSimple(sie);
                    break;
            }
        }
    }

    public override void Draw(GameTime gameTime)
    {
        Color defaultColor = Color.White;
        Color selectedColor = Color.Red;
        float rotationZero = 0f; 
        float scaleOne = 1f;
        int uiLayer = 0;

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        foreach (var (obj, texture) in _textures)
        {
            Rectangle boundingBox;
            if (obj is SimpleUIElement)
            {
                boundingBox = _boundingBoxes[obj as SimpleUIElement];
            }
            else
            {
                boundingBox = _boundingBoxes[obj as ComplexUIElement];
            }
            Vector2 pos = new Vector2(boundingBox.X, boundingBox.Y);

            if (!obj.Visible) continue;
            _spriteBatch.Draw(
                texture,
                pos + obj.TexturePosition,
                obj.TextureRect,
                Color.White
            );
        }

        foreach (var (obj, label) in _labels)
        {
            bool selected = false;
            Rectangle boundingBox;
            if (obj is SimpleUIElement s)
            {
                boundingBox = _boundingBoxes[obj as SimpleUIElement];
                selected = s.Selectable && s.Selected;
            }
            else if (obj is ComplexUIElement c)
            {
                boundingBox = _boundingBoxes[obj as ComplexUIElement];
                selected = c.Selectable && c.Selected;
            } else {
                throw new Exception("Error with bounding boxes (not simple or complex element)");
            }
            Vector2 pos = new Vector2(boundingBox.X, boundingBox.Y);
            _spriteBatch.DrawString(
                _spriteFont,
                label,
                pos + obj.TextPosition,
                selected ? selectedColor : defaultColor,
                rotationZero,
                Vector2.One,
                scaleOne,
                SpriteEffects.None,
                uiLayer);
        }
        // Handle UIHorizontalList specially
        // if (obj is UIHorizontalList horizontalList) {
        //     horizontalList.Draw(_spriteBatch);
        //     continue;
        // }

        // string label = obj.String;

        // if (label != null) {
        //     _spriteBatch.DrawString(
        //         _spriteFont, 
        //         label, 
        //         obj.TextPosition, 
        //         Color.White,
        //         0f,
        //         Vector2.One,
        //         1f,
        //         SpriteEffects.None,
        //         0);
        // }

        // if (texture != null) {
        //     _spriteBatch.Draw(texture, new Rectangle(){X = 0, Y = 0, Width = texture.Width, Height = texture.Height}, Color.White);
        // }
        _spriteBatch.End();
    }
}
