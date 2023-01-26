using UnityEditor;
using UnityEngine;

namespace MyEditor
{
    public enum EFocusMode
    {
        Default,
        Lock
    }

    public abstract class AutoEditor : Editor
    {
        protected EFocusMode focusMode;
        protected Event currentEvent;
        protected Ray mouseRay;
        protected Vector2 mousePosition;
        protected bool foldout_debug;

        /// <summary>
        /// Scene�����У���Ļ�����£�������������㵽������ߵľ���
        /// </summary>
        protected float DistanceToMouseRay(Vector3 worldPoint)
        {
            Vector3 origin = mouseRay.origin;
            float d = Mathf.Abs(worldPoint.x - origin.x) + Mathf.Abs(worldPoint.y - origin.y) + Mathf.Abs(worldPoint.z - origin.z); //����ֵ��Ȼ�����·������������С��ֵ
            return HandleUtility.DistancePointLine(worldPoint, mouseRay.origin, mouseRay.origin + d * mouseRay.direction);
        }

        /// <summary>
        /// Scene�����У���Ļ�����£�������������㵽���λ�õľ���
        /// </summary>
        protected float DistanceToMousePosition(Vector3 worldPoint)
        {
            Vector2 screenPoint = HandleUtility.WorldToGUIPoint(worldPoint);
            return (screenPoint - mousePosition).magnitude;
        }

        protected virtual void OnEnable()
        {
            if (target != null)
                AutoPropertyAttribute.Apply(this, serializedObject);
            foldout_debug = false;
        }

        /// <summary>
        /// ͨ������Ӧ����д�˷�������Ӧ����дMyOnInspectorGUI
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.BeginVertical();
            System.Type targetType = target.GetType();
            Object obj = MyEditorUtility.FindMonoScrpit(targetType);
            if (obj != null)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField("Script", obj, targetType, false);
                EditorGUI.EndDisabledGroup();
            }

            MyOnInspectorGUI();
            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }

        protected abstract void MyOnInspectorGUI();

        protected virtual void OnSceneGUI()
        {
            serializedObject.Update();
            currentEvent = Event.current;
            mousePosition = Event.current.mousePosition;
            mouseRay = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
            switch (currentEvent.type)
            {
                case EventType.Layout:
                    switch (focusMode)
                    {
                        case EFocusMode.Default:
                            break;
                        case EFocusMode.Lock:
                            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                            break;
                    }
                    break;
            }
            MyOnSceneGUI();
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void MyOnSceneGUI() { }
    }
}