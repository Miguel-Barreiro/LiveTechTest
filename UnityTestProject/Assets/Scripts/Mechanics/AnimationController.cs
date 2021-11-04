using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// AnimationController integrates physics and animation. It is generally used for simple enemy animation.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
    public class AnimationController : KinematicObject
    {
        
        /// <summary>
        /// Max horizontal speed.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Max jump velocity
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        /// <summary>
        /// Used to indicated desired direction of travel.
        /// </summary>
        public Vector2 move;

        /// <summary>
        /// Set to true to initiate a jump.
        /// </summary>
        public bool jump;

        /// <summary>
        /// Set to true to set the current jump velocity to zero.
        /// </summary>
        public bool stopJump;

        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        readonly PlatformerModel _model = Simulation.GetModel<PlatformerModel>();

        
        public static readonly int GROUNDED_PARAMETER = Animator.StringToHash("grounded");
        public static readonly int VELOCITY_X_PARAMETER = Animator.StringToHash("velocityX");

        protected virtual void Awake()
        {
            base.Awake();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * _model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * _model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                _spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                _spriteRenderer.flipX = true;

            _animator.SetBool(GROUNDED_PARAMETER, IsGrounded);
            _animator.SetFloat(VELOCITY_X_PARAMETER, Mathf.Abs(velocity.x) / maxSpeed);
                      
            targetVelocity = move * maxSpeed;
        }
        
        
        
    }
}