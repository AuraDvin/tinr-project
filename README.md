# Knight x Knives 
2D platformer Written in .net9.0 with MonoGame 

A knight tries to overthrow the Castle overrun by monsters, only using their throwing knvies to fight back. 
## Knight 
Has a throwing attack, and the platformer protagonist 
## Blobi 
A stationary enemy that shoots the player when in view 
## TBN (To be named)
A Flying enemy that tries to sweep in and attack the player from close 

Engine and game code written by me with help from professors 

# About
The game is written to be as modular as possible with scalability, basic game modules include 
- Physics engine + Debug Physics rendering engine
- Rendering engine
- Levels/Scenes system (not finalized)
- Sound engine (not yet implemented)

The idea of the project is to write such code where removing a module would not remove funtionality from other modules, for example removing the rendering engine would leave the game still playable, it just wouldn't be seen 
To draw objects they need to have the `IDrawableGameComponent` interface which `GameRenderer2D` recognizes when iterating over the scene. Then `SpriteFactory` returns the required sprite to be drawn. With each update we make sure that any objects that weren't updated have their sprite removed.
With a similar idea objects have `IPhysicsObject` interface to tell the `PhysicsEngine` they need a physics collision shape and so on. 

To match what kind of shapes or sprites an object needs, enums are used. 

# Build and run the project
```bash
# Clone the repository
git clone https://github.com/AuraDvin/ProjectTINR.git
# Move into the repository
cd ProjectTINR
# Build the project
dotnet build
# Move into output directory
cd bin/Debug/net9.0/
# Run resulting dlll
dotnet ./ProjectTINR.dll
```
