using System;
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
    [RequireComponent(typeof(CustomDebug))]
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

        private void Awake()
        {
            control = GetComponent<AnimationController>();
            _collider = GetComponent<Collider2D>();
            _audio = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            CustomDebug customDebug = GetComponent<CustomDebug>();
            customDebug.SetDebugLikes(new string[] { "Apples", "Cheese", "Malmite", "Bacon", "Milk", "Carrots", "Music" });
            customDebug.SetDebugNames(new string[] { "Albert", "Robert", "James", "Harry", "David" });
            customDebug.SetAttributes(GameConstants.EnemyTypeName, GameConstants.EnemyNameDeclaration, GameConstants.EnemyLikeDeclaration);
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