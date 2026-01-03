using System;

using Microsoft.Xna.Framework;

using ProjectTINR.Classes.NPCs;
using ProjectTINR.Classes.Objects;
using ProjectTINR.Classes.ObjectsComponents;
using ProjectTINR.Classes.Physics.Shapes;

namespace ProjectTINR.Classes.Controllers;

public class FlyingEnemyController(Game game) : GameObject(game), IController, ISceneManipulator {
    enum State { Patrol, Chase, Dive, Retreat }
    private State _state = State.Patrol;
    private float _stateTimer = 0f;
    private Vector2 _patrolTarget = Vector2.Zero;
    private Random _rand = new();

    public Scene Scene { get; set; } = null;
    public GameObject Owner { get; set; }

    public override void Initialize() { }

    public override void Update(GameTime gameTime) {
        if (Scene == null) return;
        FlyingEnemy enemy = Owner as FlyingEnemy ?? throw new NullReferenceException("Owner is not FlyingEnemy");
        Player player = Scene.FindByType<Player>();
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _stateTimer += dt;

        float patrolSpeed = 60f;
        float chaseSpeed = 180f;
        float diveSpeed = 450f;
        float retreatSpeed = 200f;
        float attackRange = 80f;

        switch (_state) {
            case State.Patrol:
                // pick a new patrol target every few seconds
                if (_patrolTarget == Vector2.Zero || _stateTimer > 3f) {
                    _stateTimer = 0f;
                    _patrolTarget = enemy.Position + new Vector2((float)(_rand.NextDouble() * 400 - 200), (float)(_rand.NextDouble() * 200 - 100));
                }
                var dir = _patrolTarget - enemy.Position;
                if (dir.LengthSquared() < 25f) {
                    _patrolTarget = Vector2.Zero;
                } else {
                    enemy.Velocity = Vector2.Normalize(dir) * patrolSpeed;
                }

                if (enemy.SeesPlayer && player != null) {
                    _state = State.Chase;
                    _stateTimer = 0f;
                }
                break;
            case State.Chase:
                if (player == null) { _state = State.Patrol; break; }
                var dirToPlayer = player.Position - enemy.Position;
                float dist = dirToPlayer.Length();
                if (dist < attackRange) {
                    _state = State.Dive;
                    _stateTimer = 0f;
                } else {
                    enemy.Velocity = Vector2.Normalize(dirToPlayer) * chaseSpeed;
                }

                if (!enemy.SeesPlayer) {
                    if (_stateTimer > 2f) { _state = State.Patrol; _stateTimer = 0f; }
                } else _stateTimer = 0f;
                break;
            case State.Dive:
                if (player == null) { _state = State.Retreat; _stateTimer = 0f; break; }
                // perform a fast dive toward the player's current position
                var diveDir = player.Position - enemy.Position;
                enemy.Velocity = Vector2.Normalize(diveDir) * diveSpeed;
                if (_stateTimer > 0.5f) {
                    _state = State.Retreat;
                    _stateTimer = 0f;
                    var awayDir = enemy.Position - player.Position;
                    enemy.Velocity = Vector2.Normalize(awayDir) * retreatSpeed;
                }
                break;
            case State.Retreat:
                if (_stateTimer > 1.2f) {
                    _state = State.Patrol;
                    _stateTimer = 0f;
                    _patrolTarget = Vector2.Zero;
                }
                break;
        }
    }
}
