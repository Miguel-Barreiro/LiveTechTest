using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{

    /// <summary>
    /// Fired when a Player collides with an Enemy.
    /// </summary>
    /// <typeparam name="EnemyCollision"></typeparam>
    public class PlayerEnemyCollision : Simulation.Event<PlayerEnemyCollision>
    {
        public EnemyController enemy;
        public PlayerController player;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            if (player.Health.IsAlive) {
                var willHurtEnemy = player.Bounds.center.y >= enemy.Bounds.max.y;

                if (willHurtEnemy)
                {
                    var enemyHealth = enemy.GetComponent<Health>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.Decrement();
                        if (!enemyHealth.IsAlive)
                        {
                            Schedule<EnemyDeath>().enemy = enemy;
                            player.Control.Bounce(2);
                        }
                        else
                        {
                            player.Control.Bounce(7);
                        }
                    }
                    else
                    {
                        Schedule<EnemyDeath>().enemy = enemy;
                        player.Control.Bounce(2);
                    }
                }
                else {
                    var ev = Schedule<PlayerDamage>();
                    ev.player = player;
                }
            }
        }
    }
}