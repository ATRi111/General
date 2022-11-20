using MyEditor;
using UnityEditor;
using UnityEngine;

namespace Services
{
    [CustomEditor(typeof(SaveManagerBase), true)]
    public class SaveManagerBaseEditor : AutoEditor
    {
        [Auto]
        public SerializedProperty core;
        public SerializedProperty runtimeData;

        protected override void OnEnable()
        {
            base.OnEnable();
            runtimeData = core.FindPropertyRelative(nameof(runtimeData));
        }

        protected override void MyOnInspectorGUI()
        {
            runtimeData.PropertyField("运行时数据");
        }
    }
}