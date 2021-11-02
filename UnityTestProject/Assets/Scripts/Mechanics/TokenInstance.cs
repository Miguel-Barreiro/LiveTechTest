using Platformer.Gameplay;
using System.Collections;
using UnityEngine;
using static Platformer.Core.Simulation;

using Models;
 
namespace Platformer.Mechanics
{
    /// <summary>
    /// This class contains the data required for implementing token collection mechanics.
    /// It does not perform animation of the token, this is handled in a batch by the 
    /// TokenController in the scene.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class TokenInstance : MonoBehaviour
    {
        public AudioClip tokenCollectAudio;
        [Tooltip("If true, animation will start at a random position in the sequence.")]
        public bool randomAnimationStartTime = false;
        [Tooltip("List of frames that make up the animation.")]
        public Sprite[] idleAnimation, collectedAnimation;

        internal Sprite[] sprites = new Sprite[0];

        internal SpriteRenderer _renderer;

        //unique index which is assigned by the TokenController in a scene.
        internal int tokenIndex = -1;
        
        //MIGUEL:
        internal TokenController controller;
        //active frame in animation, updated by the controller.
        internal int frame = 0;

        //MIGUEL: model should be injected
        // Token model
        internal TokenModel tokenModel = new TokenModel();

        void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            if (randomAnimationStartTime)
                frame = Random.Range(0, sprites.Length);
            sprites = idleAnimation;
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
            
            
            //Miguel: we should move this to the model/event instead
            if (tokenModel.collected) return;
            
            //disable the gameObject and remove it from the controller update list.
            frame = 0;
            sprites = collectedAnimation;
            if (controller != null)
                tokenModel.collected = true;
            //send an event into the gameplay system to perform some behaviour.
            var ev = Schedule<PlayerTokenCollision>();
            ev.token = this;
            ev.player = player;
        }

        // Makes a cool token flip effect on tokens
        IEnumerator flip_token()
        {
            yield return new WaitForSeconds(Random.Range(GameConstants.TokenMinFlipDelay, GameConstants.TokenMaxFlipDelay));
            
            //MIGUEL: we need 1 continuous loop instead of various subcalls
            Component[] spriteRenderers;

            spriteRenderers = GetComponents(typeof(SpriteRenderer));

            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
                spriteRenderer.flipY = !spriteRenderer.flipY;
            StartCoroutine(flip_token());
        }

        string name1()
        {
            string[] possibleNames = new string[] { "Albert", "Robert", "James", "Harry", "David" };
            int x = Random.Range(1, 9999);

            string enemyName = possibleNames[Random.Range(0, possibleNames.Length)]+x;
            return enemyName;
        }

        string like1()
        {
            string[] possibleLikes = new string[] { "Apples", "Cheese", "Malmite", "Bacon", "Milk", "Carrots", "Music" };

            string selected = possibleLikes[Random.Range(0, possibleLikes.Length)];
            return selected;
        }
//MIGUEL: extract to another class
        string makedebug()
        {
            string debug;

            debug = "";
            debug += GameConstants.TokenTypeName;
            debug += ": ";
            debug += GameConstants.TokenNameDeclaration;
            debug += " ";
            debug += name1();
            debug += " ";
            debug += GameConstants.TokenLikeDeclaration;
            debug += " ";
            debug += like1();
            debug += " ";

            return debug;
        }

        IEnumerator debugit()
        {
            yield return new WaitForSeconds(Random.Range(2, 10));
            Debug.Log(makedebug());
            StartCoroutine(debugit());
        }
    }
}