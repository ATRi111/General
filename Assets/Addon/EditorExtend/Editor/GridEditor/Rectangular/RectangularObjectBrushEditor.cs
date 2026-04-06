using UnityEditor;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(RectangularObjectBrush))]
    public class RectangularObjectBrushEditor : ObjectBrushEditor
    {
        //[AutoProperty]
        //public SerializedProperty data;

        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
        }
    }
}