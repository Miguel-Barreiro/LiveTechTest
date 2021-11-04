using System.Collections;
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

    [RequireComponent(typeof(CustomDebug))]
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


        protected override void Awake()
        {
            base.Awake();
            
            CustomDebug customDebug = GetComponent<CustomDebug>();
            customDebug.SetDebugLikes(new string[] { "Apples", "Cheese", "Malmite", "Bacon", "Milk", "Carrots", "Music" });
            customDebug.SetDebugNames(new string[] { "Albert", "Robert", "James", "Harry", "David" });
       
            customDebug.SetAttributes( GameConstants.PlayerTypeName, 
                                      GameConstants.PlayerNameDeclaration, 
                                      GameConstants.PlayerLikeDeclaration);
            
            
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            
            health.OnZeroHealth +=OnZeroHealth;
        }

        private void OnDestroy() {
            health.OnZeroHealth -=OnZeroHealth;
        }




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
        
        
        
        
        private void OnZeroHealth() {
            var ev = Schedule<HealthIsZero>();
            ev.health = health;
        }
        

        protected override void Update()
        {
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

            UpdateJump();

            base.Update();
        }

        private void UpdateJump()
        {
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
        }

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