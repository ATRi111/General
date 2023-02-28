using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    public class InitServiceEditorWindow : EditorWindow
    {
        public string search;
        public UnityAction<Type> callBack;

        private void OnGUI()
        {
            EditorGUILayout.TextField(search);
            string[] guids = AssetDatabase.FindAssets($"{search} t:MonoScript");
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                MonoScript mono = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                Type type = null;
                if (mono != null)
                    type = mono.GetClass();
                if(type != null && type.IsSubclassOf(typeof(Service)))
                {
                    string name = path[(path.LastIndexOf('.') + 1)..];
                    if(GUILayout.Button(name))
                    {
                        callBack?.Invoke(type);
                    }
                }
            }
        }

    }
}