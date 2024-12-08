using System.Collections.Generic;
using UnityEditor;

namespace MyTool
{
    public static class AutoDictionaryDrawerHelper
    {
        public static bool OnInspectorGUI(bool foldout,string label, Dictionary<string, int> dict)
        {
            bool ret = EditorGUILayout.Foldout(foldout, label);
            if (foldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Keys");
                EditorGUILayout.LabelField("Values");
                EditorGUILayout.EndHorizontal();

                EditorGUI.BeginDisabledGroup(true);
                foreach (KeyValuePair<string, int> pair in dict)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.TextField(pair.Key);
                    EditorGUILayout.IntField(pair.Value);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel--;
            }
            return ret;
        }
    }
}