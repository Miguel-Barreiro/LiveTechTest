using System;
using Platformer.Gameplay;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Platformer.UI
{
    public class TokenCountUI : MonoBehaviour
    {
        [FormerlySerializedAs("GemValue")] [SerializeField] private Text TokensNumberValue;
        
        private void Start() {
            PlayerTokenCollision.OnExecute += OnExecutePlayerTokenCollision;
        }

        private void OnDestroy() {
            PlayerTokenCollision.OnExecute -= OnExecutePlayerTokenCollision;
        }

        private void OnExecutePlayerTokenCollision(PlayerTokenCollision ev) {
            int collectedTokensNumber = ev.tokenModel.GetCollectedTokensNumber();
            TokensNumberValue.text = $"{collectedTokensNumber}";
        }
    }
}