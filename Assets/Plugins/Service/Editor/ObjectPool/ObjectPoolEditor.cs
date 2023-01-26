using UnityEditor;
using UnityEngine;

namespace Services.ObjectPools
{
    [CustomEditor(typeof(ObjectPool))]
    public class ObjectPoolEditor : Editor
    {
        public ObjectPool objectPool;
        public SerializedProperty prefab, accumulateCount;

        private void OnEnable()
        {
            objectPool = target as ObjectPool;
            prefab = serializedObject.FindProperty(nameof(prefab));
            accumulateCount = serializedObject.FindProperty(nameof(accumulateCount));
        }

        public override void OnInspectorGUI()
        {
            if (!Application.isPlaying)
            {
                base.OnInspectorGUI();
                return;
            }

            serializedObject.Update();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(prefab, new GUIContent("Ԥ����"));
            EditorGUILayout.IntField("�ۻ�����������", accumulateCount.intValue);
            EditorGUILayout.IntField("��ǰ����������", objectPool.Count);
            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
        }
    }
}