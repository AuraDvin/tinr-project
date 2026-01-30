# Knight x Knives 
> 2D platformer Written in .net9.0 with MonoGame 

A knight tries to overthrow the Castle overrun by monsters, only using their throwing knvies to fight back. 
### Knight 
Has a throwing attack, and the platformer protagonist 
### Blobi 
A stationary enemy that shoots the player when in view 
### Still unnamed flying enemy
A Flying enemy that tries to sweep in and attack the player from close 

---

> Engine and game code written by me with help from professors 

# Prerequisites
- .net 9.0 sdk [Link](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- MonoGame [Link](https://docs.monogame.net/articles/getting_started/1_setting_up_your_os_for_development_arch.html?tabs=android)
- Keyboard with arrow keys, spacebar, F1, ESC, Enter, W, A, S, D 

# Build and debug the project
```bash
# Clone the repository
git clone https://github.com/AuraDvin/ProjectTINR.git
# Move into the repository
cd ProjectTINR
# Build the project
dotnet build ProjectTINR.sln
# Move into output directory -> You have to do this so Content can be properly loaded
cd bin/Debug/net9.0/
# Run resulting dlll
dotnet ./ProjectTINR.dll
```
# Build and run the project
```bash
# Clone the repository
git clone https://github.com/AuraDvin/ProjectTINR.git
# Move into the repository
cd ProjectTINR
```
## Publish the project
I reccomend looking at [this](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog) to make sure you publish it for your system. 


If you're on x64 these should work or you can try to replace x64 with arm/arm64
### Linux
```bash
dotnet publish ProjectTINR.sln -r linux-x64 
```
```bash
dotnet publish ProjectTINR.sln -r linux-arm64
```

### Windows
```bash
dotnet publish ProjectTINR.sln -r win-x64
```
```bash
dotnet publish ProjectTINR.sln -r win-arm64
```
### MacOS
Minimum OS version is macOS 10.12 Sierra
```bash
dotnet publish ProjectTINR.sln -r osx-x64
```
```bash
dotnet publish ProjectTINR.sln -r osx-arm64
```
---


This will make the executable of your choice in `bin/Release/net<version>/<system>/publish/`.

example: `bin/Release/net9.0/linux-x64/publish/ProjectTINR`.


Then you can run `ProjectTINR`(`.exe` and such) from the command line or double click on it if you're using a file manager.


```bash
# Move into output directory -> You have to do this so Content can be properly loaded
cd bin/Release/net9.0/linux-x64/publish
# Run resulting dlll
dotnet ./ProjectTINR.dll
```

# About
The game is written to be as modular as possible with scalability, basic game modules include 
- Physics engine 
- Debug Physics shapes rendering engine
- Rendering engine
- Ui Rendering engine
- Levels/Scenes system (not finalized)
- Sound engine (not yet implemented)

The idea of the project is to write such code where removing a module would not remove funtionality from other modules, for example removing the rendering engine would leave the game still playable, it just wouldn't be seen/heard/controllable/nothing could move or collide, etc.

To draw objects they need to have the `IDrawableGameComponent` interface which `GameRenderer2D` recognizes when iterating over the scene. Then `SpriteFactory` returns the required sprite to be drawn. With each update we make sure that any objects that weren't updated have their sprite removed.
With a similar idea objects have `IPhysicsObject` interface to tell the `PhysicsEngine` they need a physics collision shape and so on. 

To match what kind of shapes or sprites an object needs, enums are used. 



# Animated Sprites & format
To draw an animation the info about the sprite that should be drawn is needed, this is done by moving a Rectangle along the texture according to the framerate. This is updated and done in `GameRenderer2D` class. 

To get the sprite atlas I used a known KDE Krita python script by [@Falano](https://github.com/Falano) at [this link](https://github.com/Falano/kritaSpritesheetManager), the result of which I edit with [my own python script](https://github.com/AuraDvin/tinr-project/blob/main/jsonEdit.py) at the project root, that edits this format to something that fits my needs, this is then parsed in the `AnimatedSprite` class.

# Ui Graphics
To draw the ui a Render engine is used that uses a different scene -> ui scene, which contains only ui elements. (This should be merged into the one scene in the future). 

This rendering engine's scenes are somewhat similar to trees as some "Nodes" (not called that in the code) are Simple (no child nodes) or Complex (have child nodes), based on which are 

1. Simple:
    - Button
    - Slider
    - Checkbox

2. Complex:
    - VerticalBox
    - HorizontalBox 

With these `BoundingBox`es are calculated from where their position is assumed, this is done through recursively going through the Complex objects (so a Vbox could have a Hbox inside for exmaple)


# Refernces/Links
- Game engine [MonoGame](https://monogame.net/)
- Drawing software [KDE Krita](https://krita.org/en/)
- Spritesheet export plugin [kritaSpriteSheetManager](https://github.com/Falano/kritaSpritesheetManager)
- IDE(s) [Visual Studio Code](https://code.visualstudio.com/) + [JetBrains Rider](https://www.jetbrains.com/rider/) (I used both during development)
