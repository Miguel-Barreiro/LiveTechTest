using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

using Models;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        public PlayerModel playerModel = new PlayerModel();

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        /*
        ** Properties
        */
        public float MaxSpeed {
            get { return playerModel.maxSpeed; }
            set {
              playerModel.maxSpeed = value;
            }
        }

        public float JumpTakeOffSpeed {
            get { return playerModel.jumpTakeOffSpeed; }
            set {
              playerModel.jumpTakeOffSpeed = value;
            }
        }

        protected override void Update()
        {
            //MIGUEL:
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            if (controlEnabled)
            {
                //MIGUEL:
                // we do the mobile stuff
                if (Application.platform == RuntimePlatform.IPhonePlayer
                || Application.platform == RuntimePlatform.Android)
                {
                    int fingerCount = 0;

                    foreach (Touch touch in Input.touches)
                    {
                        if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                        {
                            fingerCount++;
                        }
                    }
                    if (fingerCount > 0)
                    {
                        print("User has " + fingerCount + " finger(s) touching the screen");
                    }
                }

                // update_movement_for_mobile();
                move.x = Input.GetAxis("Horizontal");
                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                    jumpState = JumpState.PrepareToJump;
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
            }
            else
            {
                move.x = 0;
            }

            // this is where we handle jumps
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    StartCoroutine(debugjump(jumpState));
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }

            base.Update();
        }
//MIGUEL:
        // OLD JUMP CODE
        // void functionJump()
        // {
        //     jump = false;
        //     switch (jumpState)
        //     {
        //         case JumpState.PrepareToJump:
        //             jumpState = JumpState.Jumping;
        //             jump = true;
        //             stopJump = false;
        //             StartCoroutine(debugjump(jumpState));
        //             break;
        //         case JumpState.Jumping:
        //             if (!IsGrounded)
        //             {
        //                 Schedule<PlayerJumped>().player = this;
        //                 jumpState = JumpState.InFlight;
        //             }
        //             break;
        //         case JumpState.InFlight:
        //             if (IsGrounded)
        //             {
        //                 Schedule<PlayerLanded>().player = this;
        //                 jumpState = JumpState.Landed;
        //             }
        //             break;
        //         case JumpState.Landed:
        //             jumpState = JumpState.Grounded;
        //             break;
        //     }
        // }

        IEnumerator debugjump(JumpState jumpState)
        {
            Debug.Log("I'm jumping!");
            yield return new WaitForSeconds(4f / 2);
            Debug.Log("I'm done jumping!");
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = JumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / MaxSpeed);

            targetVelocity = move * MaxSpeed;
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
            debug += GameConstants.PlayerTypeName;
            debug += ": ";
            debug += GameConstants.PlayerNameDeclaration;
            debug += " ";
            debug += name1();
            debug += " ";
            debug += GameConstants.PlayerLikeDeclaration;
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

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}