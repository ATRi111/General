using MyEditor;
using UnityEditor;

namespace Services
{
    [CustomEditor(typeof(SaveManagerBase), true)]
    public class SaveManagerBaseEditor : AutoEditor
    {
        [Auto]
        public SerializedProperty runtimeData;

        protected override void MyOnInspectorGUI()
        {
            runtimeData.PropertyField("运行时数据");
        }
    }
}