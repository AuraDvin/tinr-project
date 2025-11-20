using System.Collections.Generic;
using System.IO;
using System.Text.Json.Nodes;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectTINR.Classes;
using ProjectTINR.Classes.ObjectsComponents;

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

public class AnimatedSprite : Sprite, IUpdatableGameComponent {
    private readonly Dictionary<string, Animation> _animations;
    // private Animation _currentAnimation;
    private string _currentAnimationName;   
    // private AnimationFrame _currentFrame;
    private int _currentFrameIdx;
    private double _lastFrameTime;

    public AnimatedSprite(Vector2 position, Texture2D texture) : base(new Rectangle(0, 0, 0, 0), position, texture) {
        _animations = [];
    }

    public void AddAnimationFromJson(string jsonPath) {
        string jsonString = File.ReadAllText(jsonPath);
        JsonNode animationFramesNode = JsonNode.Parse(jsonString);

        foreach (var frame in animationFramesNode.AsArray()) {
            string animationName = (string)frame["filename"];
            if (!_animations.ContainsKey(animationName)) {
                Animation animation = new Animation();
                animation.Frames = [];
                animation.Looping = true;
                animation.NextAnim = "TODO";
                _animations.Add(animationName, animation);
                PlayAnimation(animationName);
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
            _animations[animationName].Frames.Add(animationFrame);
        }
        PlayAnimation("idle");
    }

    public void PlayAnimation(string animationName) {
        if (animationName == _currentAnimationName) {
            return;
        }
        if (!_animations.ContainsKey(animationName)) {
            throw new KeyNotFoundException($"Animation '{animationName}' not found.");
        }
        _currentAnimationName = animationName;
        _lastFrameTime = 0;
        _currentFrameIdx = 0;
    }

    public void Update(GameTime gameTime) {
        Animation _currentAnimation = _animations[_currentAnimationName];
        _lastFrameTime += gameTime.ElapsedGameTime.Milliseconds;
        while (_lastFrameTime >= _currentAnimation.Frames[_currentFrameIdx].Duration) {
            _lastFrameTime -= _currentAnimation.Frames[_currentFrameIdx].Duration;
            _currentFrameIdx = (_currentFrameIdx + 1) % _currentAnimation.Frames.Count;
            if (_currentFrameIdx == 0 && !_currentAnimation.Looping) {
                PlayAnimation(_currentAnimation.NextAnim);
            }
        }
        Rectangle rect = _animations[_currentAnimationName].Frames[_currentFrameIdx].Rect;
        SetRect(rect);
    }
}