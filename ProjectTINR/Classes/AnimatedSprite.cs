using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json.Nodes;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TINR.Classes;

struct AnimationFrame {
    public Rectangle Rect;
    public double Duration;
    public AnimationFrame(Rectangle rect, double dur) {
        Rect = rect;
        Duration = dur;
    }
}

struct Animation {
    public bool Looping;
    public string NextAnim;
    public List<AnimationFrame> Frames;
}

// TODO read json to map and play animation
public class AnimatedSprite : Sprite {
    private readonly Dictionary<string, Animation> _spritesheet;
    private Animation _currentAnimation;
    private AnimationFrame _currentFrame;
    private int _currentFrameIdx;
    private double _lastFrameTime;

    public AnimatedSprite(Game game, Vector2 position, Texture2D texture, string jsonPath) : base(game, new Rectangle(0,0,0,0), position, texture) {
        _spritesheet = new Dictionary<string, Animation>();
        string jsonString = File.ReadAllText(jsonPath);
        JsonNode animationFramesNode = JsonNode.Parse(jsonString);

        foreach (var frame in animationFramesNode.AsArray()) {
            string animationName = (string)frame["filename"];
            if (!_spritesheet.ContainsKey(animationName)) {
                Animation animation = new Animation();
                animation.Frames = new List<AnimationFrame>();
                animation.Looping = true;
                animation.NextAnim = "TODO";
                _spritesheet.Add(animationName, animation);
                playAnimation(animationName);
            }

            Rectangle tmp_rect = new Rectangle(
                (int)frame["frame"]["x"],
                (int)frame["frame"]["y"],
                (int)frame["frame"]["w"],
                (int)frame["frame"]["h"]);

            var durval = (double?)frame["duration"];
            double tmp_duration;
            if (durval == null) {
                tmp_duration = 1000 / 12; // 12 FPS
            }
            else {
                tmp_duration = (double)durval;
            }
            AnimationFrame animationFrame = new AnimationFrame();
            animationFrame.Duration = tmp_duration;
            animationFrame.Rect = tmp_rect;
            _spritesheet[animationName].Frames.Add(animationFrame);
        }
        playAnimation("idle");
    }

    public void playAnimation(string animationName) {
        Animation animation = _spritesheet[animationName];
        _currentAnimation = animation;
        _lastFrameTime = 0;
        _currentFrameIdx = 0;
    }

    public override void Update(GameTime gameTime) {
        _lastFrameTime += gameTime.ElapsedGameTime.Milliseconds;
        while (_lastFrameTime >= _currentAnimation.Frames[_currentFrameIdx].Duration) {
            _lastFrameTime -= _currentAnimation.Frames[_currentFrameIdx].Duration;
            _currentFrameIdx = (_currentFrameIdx + 1) % _currentAnimation.Frames.Count;
            if (_currentFrameIdx == 0 && !_currentAnimation.Looping) {
                playAnimation(_currentAnimation.NextAnim);
            }
        }
        _currentFrame = _currentAnimation.Frames[_currentFrameIdx];
        base.SetRect(_currentFrame.Rect);
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime) {
        base.Draw(gameTime);
    }
}