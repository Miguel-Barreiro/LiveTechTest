using Platformer.Core;
using Platformer.Mechanics;

namespace Platformer.Gameplay
{
    public class ResetPlayerHealth : Simulation.Event<ResetPlayerHealth>
    {
        public PlayerController player;

        public override void Execute() {
            player.Health.ResetToMax();
        }

    }
}