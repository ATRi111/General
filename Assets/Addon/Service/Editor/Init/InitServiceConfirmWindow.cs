using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    public class InitServiceConfirmWindow : EditorWindow
    {
        public UnityAction callBack;

        private void OnEnable()
        {
            minSize = new Vector2(500, 50);
            maxSize = new Vector2(500, 50);
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("将在当前场景进行Service初始化，这一操作无法撤销");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("确认"))
            {
                callBack?.Invoke();
                Close();
            }
            if (GUILayout.Button("取消"))
            {
                Close();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}