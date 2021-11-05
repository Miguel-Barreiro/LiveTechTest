using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Represebts the current vital statistics of some game entity.
    /// </summary>
    public class Health : MonoBehaviour
    {

        public event Action OnZeroHealth;
        public event Action<int, int> OnHealthChanges;
        
        /// <summary>
        /// The maximum hit points for the entity.
        /// </summary>
        public int MaxHp = 1;

        /// <summary>
        /// Indicates if the entity should be considered 'alive'.
        /// </summary>
        public bool IsAlive => _currentHp > 0;

        /// <summary>
        /// from 0 to 1 indicates the percentage of health
        /// </summary>
        /// <returns></returns>
        public float GetHealthPercentage() => _currentHp / (float)MaxHp;

        
        int _currentHp;

        
        /// <summary>
        /// Increment the HP of the entity.
        /// </summary>
        public void Increment() {
            int previousHp = _currentHp;
            _currentHp = Mathf.Clamp(_currentHp + 1, 0, MaxHp);
            if (previousHp != _currentHp) {
                OnHealthChanges?.Invoke(_currentHp, MaxHp);
            }
        }

        /// <summary>
        /// Decrement the HP of the entity. Will trigger a OnZeroHealth event when
        /// current HP reaches 0.
        /// </summary>
        public void Decrement() {

            int previousHp = _currentHp;
            _currentHp = Mathf.Clamp(_currentHp - 1, 0, MaxHp);
            if (_currentHp == 0) {
                OnZeroHealth?.Invoke();
            } else {
                if (previousHp != _currentHp) {
                    OnHealthChanges?.Invoke(_currentHp, MaxHp);
                }
            }
        }

        /// <summary>
        /// Decrement the HP of the entitiy until HP reaches 0.
        /// </summary>
        public void Die()
        {
            while (_currentHp > 0) Decrement();
        }
        
        /// <summary>
        /// sets current health to the maximum
        /// </summary>
        public void ResetToMax() {
            if (_currentHp != MaxHp) {
                _currentHp = MaxHp;
                OnHealthChanges?.Invoke(_currentHp, MaxHp);
            }
        }

        void Awake()
        {
            _currentHp = MaxHp;
        }

    }
}
