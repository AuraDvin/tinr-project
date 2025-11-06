using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TINR.Classes;

struct AnimationFrame
{
    public Rectangle _rect;
    public double duration;
}

struct Animation
{
    public bool looping;
    public string nextAnim;
    public AnimationFrame[] frames;   
    
}

// TODO read json to map and play animation
public class AnimatedSprite : Sprite
{
    private Dictionary<string, Animation> Spritesheet;
    private Animation _currentAnimation;
    private AnimationFrame _currentFrame;
    private int _currentFrameIdx;
    private double _lastFrameTime;

    public AnimatedSprite(Game game, Rectangle rect, Vector2 position, Texture2D texture) : base(game, rect, position, texture)
    {
    }

    public void playAnimation(string animationName)
    {
        Animation animation = Spritesheet[animationName];
        _currentAnimation = animation;
        _lastFrameTime = 0;
        _currentFrameIdx = 0;
    }

    public override void Update(GameTime gameTime)
    {
        _lastFrameTime += gameTime.ElapsedGameTime.Milliseconds;
        while (_lastFrameTime >= _currentAnimation.frames[_currentFrameIdx].duration)
        {
            _lastFrameTime -= _currentAnimation.frames[_currentFrameIdx].duration;
            _currentFrameIdx = (_currentFrameIdx + 1) % _currentAnimation.frames.Length;
            if (_currentFrameIdx == 0 && !_currentAnimation.looping)
            {
                playAnimation(_currentAnimation.nextAnim);
            }
        }
        _currentFrame = _currentAnimation.frames[_currentFrameIdx];
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.SetRect(_currentFrame._rect);
        base.Draw(gameTime);
    }
}