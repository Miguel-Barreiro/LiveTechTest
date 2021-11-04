using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Platformer.Mechanics
{
    public class CustomDebug : MonoBehaviour
    {
        
        private string[] _possibleNames = new string[] { "Albert", "Robert", "James", "Harry", "David" };
        private string[] _possibleLikes = new string[] { "Apples", "Cheese", "Malmite", "Bacon", "Milk", "Carrots", "Music" };
        private string _typeName = "NO_TYPE";
        private string _nameDeclaration = "NO_NAME";
        private string _likeDeclaration = "NO_LIKE";

        private void Awake() {
            StartCoroutine(PrintDebugInfoCoroutine());
        }

        public void SetAttributes(string typeName, string  nameDeclaration, string likeDeclaration) {
            _typeName = typeName;
            _nameDeclaration = nameDeclaration;
            _likeDeclaration = likeDeclaration;
        }

        public void SetDebugNames(List<string> names) {
            _possibleNames = names.ToArray();
        }

        public void SetDebugNames(string[] names) {
            _possibleNames = names;
        }

        public void SetDebugLikes(List<string> likes) {
            _possibleLikes = likes.ToArray();
        }

        public void SetDebugLikes(string[] likes) {
            _possibleLikes = likes;
        }

        private string Name1()
        {
            int x = Random.Range(1, 9999);
            string enemyName = _possibleNames[Random.Range(0, _possibleNames.Length)] + x;
            return enemyName;
        }

        private string Like1()
        {
            string selected = _possibleLikes[Random.Range(0, _possibleLikes.Length)];
            return selected;
        }

        private string MakeDebug() {
            return $"{_typeName}: {_nameDeclaration} {Name1()}  {_likeDeclaration} {Like1()} ";
        }

        IEnumerator PrintDebugInfoCoroutine()
        {
            while (true) {
                yield return new WaitForSeconds(Random.Range(2, 10));
                Debug.Log(MakeDebug());
            }
        }

    }
}