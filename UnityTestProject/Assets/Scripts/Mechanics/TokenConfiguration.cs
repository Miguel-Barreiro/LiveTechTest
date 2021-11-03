using UnityEngine;

namespace Platformer.Mechanics
{
    [CreateAssetMenu(fileName = "TokenConfiguration", menuName = "TokenConfiguration", order = 0)]
    public class TokenConfiguration : ScriptableObject
    {
        public AudioClip tokenCollectAudio;
        [Tooltip("If true, animation will start at a random position in the sequence.")]
        public bool randomAnimationStartTime = false;
        [Tooltip("List of frames that make up the animation.")]
        public Sprite[] idleAnimation, collectedAnimation;

    }
}