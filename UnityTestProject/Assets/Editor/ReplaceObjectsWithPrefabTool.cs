using UnityEditor;
using UnityEngine;

namespace Platformer
{
    public class ReplaceObjectsWithPrefabTool : EditorWindow
    {
        [MenuItem("Window/LiveTechGames/Replace With Prefab Tool")]
        private static void ShowWindow() {
            var window = GetWindow<ReplaceObjectsWithPrefabTool>();
            window.titleContent = new GUIContent("Replace With Prefab");
            window.Show();
        }

        [SerializeField] private GameObject Prefab;
        
        private void OnGUI()
        {
            Prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", Prefab, typeof(GameObject), false);

            if (GUILayout.Button("Replace"))
            {
                var selection = Selection.gameObjects;

                for (var i = selection.Length - 1; i >= 0; --i)
                {
                    var selected = selection[i];

                    var newObject = (GameObject)PrefabUtility.InstantiatePrefab(Prefab);

                    if (newObject == null)
                    {
                        Debug.LogError("Error instantiating prefab");
                        break;
                    }

                    Undo.RegisterCreatedObjectUndo(newObject, "Replace With Prefabs");
                    newObject.transform.SetParent(selected.transform.parent);
                    newObject.transform.localPosition = selected.transform.localPosition;
                    newObject.transform.localRotation = selected.transform.localRotation;
                    newObject.transform.localScale = selected.transform.localScale;
                    newObject.transform.SetSiblingIndex(selected.transform.GetSiblingIndex());
                    Undo.DestroyObjectImmediate(selected);
                }
            }

            GUI.enabled = false;
            EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);
        }    
    }
}