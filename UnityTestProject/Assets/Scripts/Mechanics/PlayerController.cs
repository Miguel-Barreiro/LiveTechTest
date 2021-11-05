using System.Collections;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

using Models;
using UnityEngine.Serialization;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>

    [RequireComponent(typeof(CustomDebug))]
    [RequireComponent(typeof(AnimationController))]
    public class PlayerController : MonoBehaviour
    {        
    
        public static int DEAD_ANIMATOR_BOOL_PARAMETER = Animator.StringToHash("dead");
        public static int VICTORY_ANIMATOR_TRIGGER_PARAMETER = Animator.StringToHash("victory");
        public static int HURT_ANIMATOR_TRIGGER_PARAMETER = Animator.StringToHash("hurt");

        public PlayerModel PlayerModel = Simulation.GetModel<PlayerModel>();
        
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        internal  JumpState jumpState = JumpState.Grounded;
        
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        internal Health Health;
        internal bool ControlEnabled = true;
        
        
        public AnimationController Control;
        public SpriteRenderer spriteRenderer;
        public Animator Animator;
        public readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        protected  void Awake() {

            
            CustomDebug customDebug = GetComponent<CustomDebug>();
            customDebug.SetDebugLikes(new string[] { "Apples", "Cheese", "Malmite", "Bacon", "Milk", "Carrots", "Music" });
            customDebug.SetDebugNames(new string[] { "Albert", "Robert", "James", "Harry", "David" });
       
            customDebug.SetAttributes( GameConstants.PlayerTypeName, 
                                      GameConstants.PlayerNameDeclaration, 
                                      GameConstants.PlayerLikeDeclaration);
            
            
            Control = GetComponent<AnimationController>();
            Health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            Animator = GetComponent<Animator>();
            
            Health.OnZeroHealth +=OnZeroHealth;
        }

        
        private void OnDestroy() {
            Health.OnZeroHealth -=OnZeroHealth;
        }


        /*
        ** Properties
        */
        public float MaxSpeed {
            get { return PlayerModel.maxSpeed; }
            set {
              PlayerModel.maxSpeed = value;
              Control.maxSpeed = value;
            }
        }

        public float JumpTakeOffSpeed {
            get { return PlayerModel.jumpTakeOffSpeed; }
            set {
              PlayerModel.jumpTakeOffSpeed = value;
              Control.jumpTakeOffSpeed = value;
            }
        }
        
        
        private void OnZeroHealth() {
            var ev = Schedule<HealthIsZero>();
            ev.health = Health;
        }


        protected void Update()
        {
            if (ControlEnabled)
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
                Control.move.x = Input.GetAxis("Horizontal");
                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                    jumpState = JumpState.PrepareToJump;
                else if (Input.GetButtonUp("Jump"))
                {
                    Control.stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
            }
            else
            {
                Control.move.x = 0;
            }

            UpdateJump();
        }

        private void UpdateJump()
        {
            Control.jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    Control.jump = true;
                    Control.stopJump = false;
                    StartCoroutine(debugjump(jumpState));
                    break;
                case JumpState.Jumping:
                    if (!Control.IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (Control.IsGrounded)
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