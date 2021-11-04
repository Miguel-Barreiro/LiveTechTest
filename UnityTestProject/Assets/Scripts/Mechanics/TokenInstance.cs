using System;
using Platformer.Gameplay;
using System.Collections;
using UnityEngine;
using static Platformer.Core.Simulation;

using Models;
using Random = UnityEngine.Random;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This class contains the data required for implementing token collection mechanics.
    /// It does not perform animation of the token, this is handled in a batch by the 
    /// TokenController in the scene.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(CustomDebug))]
    public class TokenInstance : MonoBehaviour
    {

        public event Action<TokenInstance> OnCollected;

        public TokenConfiguration TokenConfiguration;
        internal Sprite[] sprites = null;
        internal SpriteRenderer Renderer;
        
        //active frame in animation, updated by the controller.
        internal int frame = 0;

        // Token model
        internal TokenModel tokenModel = new TokenModel();

        void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();

            CustomDebug customDebug = GetComponent<CustomDebug>();
            customDebug.SetDebugLikes(new string[] { "Apples", "Cheese", "Malmite", "Bacon", "Milk", "Carrots", "Music" });
            customDebug.SetDebugNames(new string[] { "Albert", "Robert", "James", "Harry", "David" });
            customDebug.SetAttributes(GameConstants.TokenTypeName, GameConstants.TokenNameDeclaration, GameConstants.TokenLikeDeclaration);
            
            sprites = TokenConfiguration.idleAnimation;
            
            if (TokenConfiguration.randomAnimationStartTime)
                frame = Random.Range(0, sprites.Length);

        }

        void Start()
        {
            StartCoroutine(flip_token());
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            //only exectue OnPlayerEnter if the player collides with this token.
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null) OnPlayerEnter(player);
        }

        void OnPlayerEnter(PlayerController player)
        {
            if (tokenModel.collected) return;

            tokenModel.collected = true;

            frame = 0;
            sprites = TokenConfiguration.collectedAnimation;
            OnCollected?.Invoke(this);

            //send an event into the gameplay system to perform some behaviour.
            var ev = Schedule<PlayerTokenCollision>();
            ev.token = this;
            ev.player = player;
        }

        // Makes a cool token flip effect on tokens
        IEnumerator flip_token()
        {
            while (true) {
                yield return new WaitForSeconds(Random.Range(GameConstants.TokenMinFlipDelay, GameConstants.TokenMaxFlipDelay));
                Renderer.flipY = !Renderer.flipY;
            }
        }

    }
}