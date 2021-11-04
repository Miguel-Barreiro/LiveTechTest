using UnityEngine;
using Platformer.Gameplay;

[RequireComponent(typeof(ParticleSystem))]
public class EmitParticlesOnLand : MonoBehaviour
{

    [SerializeField]
    private bool emitOnLand = true;
    [SerializeField]
    private bool emitOnEnemyDeath = true;

#if UNITY_TEMPLATE_PLATFORMER

    private ParticleSystem _particleSystem;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();

        if (emitOnLand) {
            Platformer.Gameplay.PlayerLanded.OnExecute += OnExecuteOnPlayerLand;
        }

        if (emitOnEnemyDeath) {
            Platformer.Gameplay.EnemyDeath.OnExecute += OnExecuteEnemyDeath;
        }
    }

    private void OnExecuteEnemyDeath(Platformer.Gameplay.EnemyDeath obj) {
        _particleSystem.Play();
    }

    
    private void OnExecuteOnPlayerLand(PlayerLanded obj) {
        _particleSystem.Play();
    }

    private void OnDestroy() {
        if (emitOnLand) {
            Platformer.Gameplay.PlayerLanded.OnExecute -= OnExecuteOnPlayerLand;
        }

        if (emitOnEnemyDeath) {
            Platformer.Gameplay.EnemyDeath.OnExecute -= OnExecuteEnemyDeath;
        }
        
    }

#endif

}
