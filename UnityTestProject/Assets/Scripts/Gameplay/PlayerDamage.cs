using Platformer.Core;
using Platformer.Mechanics;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{
    public class PlayerDamage : Simulation.Event<PlayerDamage>
    {
        public PlayerController player;

        public override void Execute() {
            
            var playerHealth = player.Health;
            if (playerHealth != null)
            {
                playerHealth.Decrement();
                if (!playerHealth.IsAlive)
                {
                    Schedule<PlayerDeath>();
                }
                else
                {
                    if (player.audioSource && player.ouchAudio)
                        player.audioSource.PlayOneShot(player.ouchAudio);
                    player.Control.Bounce(7);
                }
            }
            else
            {
                Schedule<PlayerDeath>();
            }
        }

    }
}