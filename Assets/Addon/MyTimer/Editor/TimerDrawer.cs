using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace MyTimer
{
    [CustomPropertyDrawer(typeof(TimerBase), true)]
    public class TimerDrawer : AutoPropertyDrawer
    {
        [AutoProperty]
        public SerializedProperty paused, completed, time, duration;

        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginDisabledGroup(true);
            paused.BoolField("计时暂停", NextRectRelative());
            completed.BoolField("计时完成", NextRectRelative());
            duration.FloatField("总时间", NextRectRelative());
            time.FloatField("当前时间", NextRectRelative());
            EditorGUI.EndDisabledGroup();
        }
    }
}