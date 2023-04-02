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
            EditorGUILayout.LabelField("���ڵ�ǰ��������Service��ʼ������һ�����޷�����");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("ȷ��"))
            {
                callBack?.Invoke();
                Close();
            }
            if (GUILayout.Button("ȡ��"))
            {
                Close();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}