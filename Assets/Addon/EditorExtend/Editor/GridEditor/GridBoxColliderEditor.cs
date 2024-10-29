using UnityEditor;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(GridBoxCollider))]
    public class GridBoxColliderEditor : GridColliderEditor
    {
        [AutoProperty]
        public SerializedProperty height;

        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
            height.FloatField("¸ß¶È");
        }
    }
}