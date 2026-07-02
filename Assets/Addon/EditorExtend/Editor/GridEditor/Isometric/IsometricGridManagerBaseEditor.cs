using UnityEditor;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(IsometricGridManagerBase))]
    public class IsometricGridManagerBaseEditor : GridManagerBaseEditor
    {
        [AutoProperty]
        public SerializedProperty maxLayer;

        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
            EditorGUI.BeginDisabledGroup(true);
            maxLayer.IntField("最高层数");
            EditorGUI.EndDisabledGroup();
        }
    }
}