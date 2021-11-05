using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player has died.
    /// </summary>
    /// <typeparam name="PlayerDeath"></typeparam>
    public class PlayerDeath : Simulation.Event<PlayerDeath>
    {
        private PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            
            var player = GameController.Instance.Player;
            if (player.Health.IsAlive)
            {
                player.Health.Die();
            }
            
            GameController.Instance.VirtualCamera.m_Follow = null;
            GameController.Instance.VirtualCamera.m_LookAt = null;
            
            // player.collider2d.enabled = false;
            player.ControlEnabled = false;

            if (player.audioSource && player.ouchAudio)
                player.audioSource.PlayOneShot(player.ouchAudio);

            player.Animator.SetTrigger(PlayerController.HURT_ANIMATOR_TRIGGER_PARAMETER);
            player.Animator.SetBool(PlayerController.DEAD_ANIMATOR_BOOL_PARAMETER, true);
            Simulation.Schedule<PlayerSpawn>(2);
        }
    }
}