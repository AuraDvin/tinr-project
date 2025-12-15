using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.NPCs;
using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics.Shapes;

namespace ProjectTINR.Classes.Controllers;

public class EnemyController(Game game) : GameObject(game), IController, ISceneManipulator {
    private float _shootingDelay = 10f;
    private float _lastShot = 0f;
    private bool _canShoot = true;

    public bool JustJumped => false;
    public bool IsMovingLeft => false;
    public bool IsMovingRight => false;
    
    public Scene Scene { get; set; } = null;

    public override void Initialize() { }

    public override void Update(GameTime gameTime) {
        // Console.WriteLine("Updated Enemy controller");
        if (Scene == null) {
            Console.WriteLine("[Enemy Controller] Scene is null!");
            return;
        }

        _lastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_lastShot >= _shootingDelay) {
            _canShoot = true;
        }

        StationaryEnemy enemy = Owner as StationaryEnemy ??  throw new NullReferenceException("EnemyCS doesn't have owner reference");

        if (!enemy.SeesPlayer) return;
        if (!_canShoot) return;

        Player player = Scene.FindByType<Player>();
        if (player == null) return;

        int dir = player.Position.X >= enemy.Position.X ? 1 : -1;
        // int dir = -1;

        Vector2 spawnPos = enemy.Position;
        EnemyProjectile projectile = new (Game) {
            Position = spawnPos,
            FacingRight = player.Position.X >= enemy.Position.X,
        };
        Scene.Add(projectile);
        _canShoot = false;
        _lastShot = 0f;
    }

    public GameObject Owner { get; set; }
}
