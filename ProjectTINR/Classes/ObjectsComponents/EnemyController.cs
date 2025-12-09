using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.NPCs;
using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes;
using ProjectTINR.Classes.Physics.Shapes;

namespace ProjectTINR.Classes.ObjectsComponents;

public class EnemyController(Game game) : GameObject(game), IController, ISceneManipulator {
    private float _shootingDelay = 1.2f;
    private float _lastShot = 0f;
    private bool _canShoot = true;

    public bool JustJumped => false;
    public bool IsMovingLeft => false;
    public bool IsMovingRight => false;

    public StationaryEnemy Owner;
    public Scene Scene { get; set; } = null;

    public override void Initialize() { }

    public override void Update(GameTime gameTime) {
        Console.WriteLine("Updated Enemy controller");
        if (Scene == null) {
            Console.WriteLine("Scene is null!");
            return;
        }

        _lastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_lastShot >= _shootingDelay) {
            _canShoot = true;
        }

        // Find a stationary enemy (first one) and make it shoot when it sees the player
        StationaryEnemy enemy = Owner;
        // if (enemy == null) return;

        if (!enemy.SeesPlayer) return;
        if (!_canShoot) return;

        Player player = Scene.FindByType<Player>();
        if (player == null) return;

        int dir = player.Position.X >= enemy.Position.X ? 1 : -1;
        // int dir = -1;
        Vector2 spawnPos = enemy.Position;

        ProjectileCollisionShape proj = new(spawnPos, dir, Game) {
            Scene = Scene
        };
        _canShoot = false;
        _lastShot = 0f;
    }
}
