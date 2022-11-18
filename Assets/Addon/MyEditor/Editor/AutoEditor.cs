using System.Reflection;
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

        protected virtual void OnEnable()
        {
            FieldInfo[] infos = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (target != null)
            {
                foreach (FieldInfo info in infos)
                {
                    if (AutoAttribute.TryGetPropertyName(info, out string name))
                    {
                        SerializedProperty temp = serializedObject.FindProperty(name);
                        if(temp != null)
                            info.SetValue(this, temp);
                        else
                            Debug.Log($"{target.GetType()}找不到名为{name}的字段");
                    }
                }
            }
        }

        /// <summary>
        /// 通常，不应该重写此方法，而应该重写MyOnInspectorGUI
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
            currentEvent = Event.current;
            mouseRay = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
            switch (currentEvent.type)
            {
                //待完善
                case EventType.Layout:
                    switch (focusMode)
                    {
                        case EFocusMode.Lock:
                            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                            break;
                    }
                    break;
            }
            MyOnSceneGUI();
        }

        protected virtual void MyOnSceneGUI() { }
    }
}