using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Services
{
    [CustomEditor(typeof(InitService))]
    public class InitServiceEditor : Editor
    {
        public SerializedProperty search;
        private List<Type> searchResult;

        private void OnEnable()
        {
            searchResult = new List<Type>();
            search = serializedObject.FindProperty(nameof(search));
            string[] temp = AssetDatabase.FindAssets($"t:MonoScript");
            for (int i = 0; i < temp.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(temp[i]);
                MonoScript mono = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                Type type = null;
                if (mono != null)
                    type = mono.GetClass();
                if (type != null && type.IsSubclassOf(typeof(Service)))
                    searchResult.Add(type);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            for (int i = 0; i < searchResult.Count; i++)
            {
                Type type = searchResult[i];
                if (type.ToString().Contains(search.stringValue, StringComparison.InvariantCultureIgnoreCase))
                {
                    string name = type.ToString();
                    if (GUILayout.Button(name))
                    {
                        Undo.AddComponent((target as Component).gameObject, type);
                        Undo.DestroyObjectImmediate(target);
                        return;
                    }
                }
            }
        }
    }
}