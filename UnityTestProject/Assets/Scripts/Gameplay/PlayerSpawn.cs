using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player is spawned after dying.
    /// </summary>
    public class PlayerSpawn : Simulation.Event<PlayerSpawn>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = GameController.Instance.Player;

            player.collider2d.enabled = true;
            player.ControlEnabled = false;

            if (player.audioSource && player.respawnAudio)
                player.audioSource.PlayOneShot(player.respawnAudio);


            var ev = Simulation.Schedule<ResetPlayerHealth>();
            ev.player = player;

            player.Control.Teleport(GameController.Instance.spawnPoint.transform.position);
            player.jumpState = PlayerController.JumpState.Grounded;

            player.Animator.SetBool(PlayerController.DEAD_ANIMATOR_BOOL_PARAMETER, false);
            
            GameController.Instance.VirtualCamera.m_Follow = player.transform;
            GameController.Instance.VirtualCamera.m_LookAt = player.transform;
            
            Simulation.Schedule<EnablePlayerInput>(2f);
        }
    }
}