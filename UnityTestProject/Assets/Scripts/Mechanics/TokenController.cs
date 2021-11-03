using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Models;
using Random = UnityEngine.Random;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This class animates all token instances in a scene.
    /// This allows a single update call to animate hundreds of sprite 
    /// animations.
    /// If the tokens property is empty, it will automatically find and load 
    /// all token instances in the scene at runtime.
    /// </summary>
    public class TokenController : MonoBehaviour
    {
        [Tooltip("Frames per second at which tokens are animated.")]
        public float frameRate = 12;
        
        [Tooltip("Instances of tokens which are animated. If empty, token instances are found and loaded at runtime.")] 
        [SerializeField]
        private List<TokenInstance> tokens = new List<TokenInstance>();
        private readonly List<TokenInstance> _collectingTokens = new List<TokenInstance>();
        private readonly List<TokenInstance> _collectedTokens = new List<TokenInstance>();


        [ContextMenu("Find All Tokens")]
        void FindAllTokensInScene()
        {
            tokens.AddRange(UnityEngine.Object.FindObjectsOfType<TokenInstance>());
        }

        void Awake()
        {
            //if tokens are empty, find all instances.
            //if tokens are not empty, they've been added at editor time.
            if (tokens.Count == 0)
                FindAllTokensInScene();
            
            //Register all tokens so they can work with this controller.
            foreach (TokenInstance tokenInstance in tokens) {
                tokenInstance.OnCollected += OnTokenCollected;
                
                if (tokenInstance.TokenConfiguration.randomAnimationStartTime)
                    tokenInstance.frame = Random.Range(0, tokenInstance.sprites.Length);
            }

            StartCoroutine(UpdateTokenSpritesCoroutine());
        }


        private void OnTokenCollected(TokenInstance token) {
            
            token.frame = 0;
            _collectingTokens.Add(token);

            token.OnCollected -= OnTokenCollected;
        }

        private IEnumerator UpdateTokenSpritesCoroutine() {

            float framesDeltaTime = 1f / frameRate;
            
            while (true) {
                
                 yield return new WaitForSeconds(framesDeltaTime);

                //update all tokens with the next animation frame.
                foreach (TokenInstance token in tokens) {
                    token._renderer.sprite = token.sprites[token.frame];
                    token.frame = (token.frame + 1) % token.sprites.Length;
                }

                foreach (TokenInstance token in _collectingTokens) {
                    if (token.frame == token.sprites.Length - 1){
                        
                        //we could destroy it but I let it remain to keep the same functionality
                        token.gameObject.SetActive(false);

                        _collectedTokens.Add(token);
                        
                        //lets remove from the tokens that need to be animated though
                        tokens.Remove(token);
                    }
                }

                _collectingTokens.RemoveAll(instance => !instance.gameObject.activeSelf);
            }
        }
    }
}