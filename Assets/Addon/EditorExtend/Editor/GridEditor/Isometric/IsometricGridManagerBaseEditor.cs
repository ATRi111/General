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
            maxLayer.IntField("×î¸ß²ãÊý");
        }
    }
}