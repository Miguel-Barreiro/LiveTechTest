using System;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Represebts the current vital statistics of some game entity.
    /// </summary>
    public class Health : MonoBehaviour
    {

        public event Action OnZeroHealth;
        
        /// <summary>
        /// The maximum hit points for the entity.
        /// </summary>
        public int maxHP = 1;

        /// <summary>
        /// Indicates if the entity should be considered 'alive'.
        /// </summary>
        public bool IsAlive => _currentHp > 0;

        int _currentHp;

        /// <summary>
        /// Increment the HP of the entity.
        /// </summary>
        public void Increment()
        {
            _currentHp = Mathf.Clamp(_currentHp + 1, 0, maxHP);
        }

        /// <summary>
        /// Decrement the HP of the entity. Will trigger a OnZeroHealth event when
        /// current HP reaches 0.
        /// </summary>
        public void Decrement()
        {
            _currentHp = Mathf.Clamp(_currentHp - 1, 0, maxHP);
            if (_currentHp == 0)
            {
                OnZeroHealth?.Invoke();
            }
        }

        /// <summary>
        /// Decrement the HP of the entitiy until HP reaches 0.
        /// </summary>
        public void Die()
        {
            while (_currentHp > 0) Decrement();
        }

        void Awake()
        {
            _currentHp = maxHP;
        }
    }
}
