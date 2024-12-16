using EditorExtend;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Services
{
    [CustomEditor(typeof(InitService))]
    public class InitServiceEditor : AutoEditor
    {
        [AutoProperty]
        public SerializedProperty search;
        private List<Type> searchResult;

        protected override void OnEnable()
        {
            base.OnEnable();
            searchResult = new List<Type>();
            EditorExtendUtility.FindAllScriptInherit(typeof(Service), searchResult);
        }

        protected override void MyOnInspectorGUI()
        {
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