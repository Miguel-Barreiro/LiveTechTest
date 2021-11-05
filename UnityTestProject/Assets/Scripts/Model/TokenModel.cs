using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

namespace Models
{
    [System.Serializable]
    public class TokenModel
    {

        private readonly Dictionary<Vector2, bool> _tokensCollected = new Dictionary<Vector2, bool>();

        public bool Collected(Vector2 tokenPosition) {
            if (!_tokensCollected.ContainsKey(tokenPosition)) {
                _tokensCollected.Add(tokenPosition, false);
            }
            return _tokensCollected[tokenPosition];
        }

        public void SetCollected(Vector2 tokenPosition, bool collected) {
            _tokensCollected[tokenPosition] = collected;
        }

        public int GetCollectedTokensNumber() {
            return _tokensCollected.Count(pair => pair.Value);
        }
    }
}