using Platformer.Core;
using Platformer.Model;
using UnityEngine;
using System.Collections.Generic;
using Platformer.Gameplay;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This class exposes the the game model in the inspector, and ticks the
    /// simulation.
    /// </summary> 
    public class GameController : MonoBehaviour
    {
        static List<float> _tickTimes = new List<float>();

        public static GameController Instance { get; private set; }

        
        /// <summary>
        /// The virtual camera in the scene.
        /// </summary>
        public Cinemachine.CinemachineVirtualCamera VirtualCamera;

        /// <summary>
        /// The main component which controls the player sprite, controlled 
        /// by the user.
        /// </summary>
        public PlayerController Player;
        

        /// <summary>
        /// The spawn point in the scene.
        /// </summary>
        public Transform spawnPoint;
        
        
        //This model field is public and can be therefore be modified in the 
        //inspector.
        //The reference actually comes from the InstanceRegister, and is shared
        //through the simulation and events. Unity will deserialize over this
        //shared reference when the scene loads, allowing the model to be
        //conveniently configured inside the inspector.
        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        void OnEnable()
        {
            Instance = this;
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this) 
            {
                Simulation.Tick();
                _tickTimes.Add(Time.deltaTime);
            }
        }
    }
}