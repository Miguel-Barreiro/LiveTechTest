using Platformer.Gameplay;
using UnityEngine;

namespace Platformer.UI
{

    public class GameplayUIController : MonoBehaviour
    {
        private void Awake() {
            PlayerEnteredVictoryZone.OnExecute += OnExecutePlayerEnteredVictoryZone;
        }

        private void OnDestroy() {
            PlayerEnteredVictoryZone.OnExecute -= OnExecutePlayerEnteredVictoryZone;
        }

        private void OnExecutePlayerEnteredVictoryZone(PlayerEnteredVictoryZone ev) {
            gameObject.SetActive(false);
        }

    }
}