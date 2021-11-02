using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// A simple controller for enemies. Provides movement control over a patrol path.
    /// </summary>
    [RequireComponent(typeof(AnimationController), typeof(Collider2D))]
    public class EnemyController : MonoBehaviour
    {
        public PatrolPath path;
        public AudioClip ouch;

        internal PatrolPath.Mover mover;
        internal AnimationController control;
        internal Collider2D _collider;
        internal AudioSource _audio;
        SpriteRenderer spriteRenderer;

        public Bounds Bounds => _collider.bounds;

        static List<float> _newPositions = new List<float>();

        void Awake()
        {
            control = GetComponent<AnimationController>();
            _collider = GetComponent<Collider2D>();
            _audio = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            StartCoroutine(debugit());
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                var ev = Schedule<PlayerEnemyCollision>();
                ev.player = player;
                ev.enemy = this;
            }
        }

        //MIGUEL:
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

        string makedebug()
        {
            string debug;

            debug = "";
            debug += GameConstants.EnemyTypeName;
            debug += ": ";
            debug += GameConstants.EnemyNameDeclaration;
            debug += " ";
            debug += name1();
            debug += " ";
            debug += GameConstants.EnemyLikeDeclaration;
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

        void Update()
        {
            if (path != null)
            {
                if (mover == null) mover = path.CreateMover(control.maxSpeed * 0.5f);
                float newPosition = mover.Position.x - transform.position.x;
                control.move.x = Mathf.Clamp(newPosition, -1, 1);
            }
        }
    }
}