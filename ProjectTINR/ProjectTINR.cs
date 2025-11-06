using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TINR.Classes;

namespace TINR;

public class ProjectTinr : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private LinkedList<Sprite> _sprites;
    private Texture2D _texture;
    private List<Rectangle> _rects1;
    private List<Rectangle> _rects2;
    private int _animFrame = 0; 
    
    public ProjectTinr()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _sprites = new LinkedList<Sprite>();
        _rects1 = new List<Rectangle>();
        _rects2 = new List<Rectangle>();
        
        _rects1.Add(new Rectangle(0, 0, 194, 194)); 
        _rects1.Add(new Rectangle(194, 0, 194, 194)); 
        _rects1.Add(new Rectangle(388, 0, 194, 194)); 
        _rects1.Add(new Rectangle(582, 0, 194, 194)); 
        _rects1.Add(new Rectangle(776, 0, 194, 194)); 
        _rects1.Add(new Rectangle(970, 0, 194, 194)); 
        
        _rects2.Add(new Rectangle(1552, 970, 194, 194));
        _rects2.Add(new Rectangle(1746, 970, 194, 194));
        _rects2.Add(new Rectangle(1940, 970, 194, 194));
        _rects2.Add(new Rectangle(0, 1164, 194, 194));
        _rects2.Add(new Rectangle(194, 1164, 194, 194));
        _rects2.Add(new Rectangle(388, 1164, 194, 194));
        _rects2.Add(new Rectangle(582, 1164, 194, 194));
        _rects2.Add(new Rectangle(776, 1164, 194, 194));
        _rects2.Add(new Rectangle(970, 1164, 194, 194));
        _rects2.Add(new Rectangle(1164, 1164, 194, 194));
        _rects2.Add(new Rectangle(1358, 1164, 194, 194));
        _rects2.Add(new Rectangle(1552, 1164, 194, 194));
    }

    protected override void Initialize()
    {
        _texture = Content.Load<Texture2D>("chracters");
        
        _sprites.AddFirst(new Sprite(this, _rects1[0], new Vector2(420.0f, 42.0f), _texture));
        _sprites.AddFirst(new Sprite(this, _rects2[0], new Vector2(GraphicsDevice.Viewport.Width*0.5f, GraphicsDevice.Viewport.Height*0.5f), _texture));
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        if (gameTime.TotalGameTime.Milliseconds % 12 == 0)
        {
            _animFrame++;
            _sprites.First.Value.SetRect( _rects1[_animFrame % _rects1.Count]);
            _sprites.Last.Value.SetRect( _rects2[_animFrame % _rects2.Count]);
        }
     
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        foreach (var sprite in _sprites)
        {
            sprite.Draw(_spriteBatch);
        }
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}