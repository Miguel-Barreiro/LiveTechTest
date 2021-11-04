using UnityEngine;
using UnityEngine.UI;

namespace Platformer.Gameplay
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Image BarFill;

        private void SetHealthPercentage(float percentage) {
            BarFill.fillAmount = percentage;
        }
        
        private void Start() {
            ResetPlayerHealth.OnExecute += OnExecuteChangePlayerHealth;
            PlayerDamage.OnExecute += OnExecutePlayerDamage;
        }

        private void OnDestroy() {
            ResetPlayerHealth.OnExecute -= OnExecuteChangePlayerHealth;
            PlayerDamage.OnExecute -= OnExecutePlayerDamage;
        }

        private void OnExecutePlayerDamage(PlayerDamage ev) {
            SetHealthPercentage(ev.player.Health.GetHealthPercentage());
        }

        private void OnExecuteChangePlayerHealth(ResetPlayerHealth ev) {
            SetHealthPercentage(ev.player.Health.GetHealthPercentage());
        }

    }
}