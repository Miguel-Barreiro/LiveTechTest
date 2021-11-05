using System;
using Models;
using Platformer.Core;
using Platformer.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.UI
{
    
    public class VictoryScreenUI : MonoBehaviour
    {

        [SerializeField]
        private Text TokenCountText;
        
        private void Awake() { PlayerEnteredVictoryZone.OnExecute += OnExecutePlayerEnteredVictoryZone; }

        private void OnDestroy() { PlayerEnteredVictoryZone.OnExecute -= OnExecutePlayerEnteredVictoryZone; }

        private void OnExecutePlayerEnteredVictoryZone(PlayerEnteredVictoryZone ev) {
            TokenModel tokenModel = Simulation.GetModel<TokenModel>();
            TokenCountText.text = $"{tokenModel.GetCollectedTokensNumber()}";
            gameObject.SetActive(true);
        }

        void Start() { gameObject.SetActive(false); }

    }
}