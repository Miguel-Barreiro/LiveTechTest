using Models;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when a player collides with a token.
    /// </summary>
    /// <typeparam name="PlayerCollision"></typeparam>
    public class PlayerTokenCollision : Simulation.Event<PlayerTokenCollision>
    {
        public PlayerController player;
        public TokenInstance token;

        public readonly TokenModel tokenModel = Simulation.GetModel<TokenModel>();

        public override void Execute()
        {
            tokenModel.SetCollected(token.transform.position,true);
            AudioSource.PlayClipAtPoint(token.TokenConfiguration.tokenCollectAudio, token.transform.position);
        }
    }
}