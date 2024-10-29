using UnityEditor;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(GridCylinderCollider))]
    public class GridCylinderColliderEditor : GridColliderEditor
    {
        [AutoProperty]
        public SerializedProperty height, radius;

        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
            height.FloatField("¸ß¶È");
            radius.FloatField("°ë¾¶");
        }
    }
}