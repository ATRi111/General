using UnityEditor;
using UnityEngine;

namespace EditorExtend
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
        protected Object monoScript;
        protected System.Type targetType;
        /// <summary>
        /// 自定义Inspector是否要附在原本的Inspector之后，而不是覆盖原本的Inspector
        /// </summary>
        protected bool appendInspector;

        /// <summary>
        /// Scene窗口中，屏幕坐标下，给定世界坐标点到鼠标射线的距离
        /// </summary>
        protected float DistanceToMouseRay(Vector3 worldPoint)
        {
            Vector3 origin = mouseRay.origin;
            float d = Mathf.Abs(worldPoint.x - origin.x) + Mathf.Abs(worldPoint.y - origin.y) + Mathf.Abs(worldPoint.z - origin.z); //此数值必然大于下方函数所需的最小数值
            return HandleUtility.DistancePointLine(worldPoint, mouseRay.origin, mouseRay.origin + d * mouseRay.direction);
        }

        /// <summary>
        /// Scene窗口中，屏幕坐标下，给定世界坐标点到鼠标位置的距离
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
            targetType = target.GetType();
            monoScript = EditorExtendUtility.FindMonoScrpit(targetType);
        }

        /// <summary>
        /// 通常，不应该重写此方法，而应该重写MyOnInspectorGUI
        /// </summary>
        public override void OnInspectorGUI()
        {
            if (appendInspector)
                base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.BeginVertical();
            if (monoScript != null)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField("Script", monoScript, targetType, false);
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