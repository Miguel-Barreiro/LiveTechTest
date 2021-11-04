using UnityEngine;

namespace Models
{
    [System.Serializable]
    public class PlayerModel
    {
        
        
        
    	/// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = GameConstants.PlayerMaxSpeed;

        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = GameConstants.PlayerJumpTakeOffSpeed;

        
    }
}