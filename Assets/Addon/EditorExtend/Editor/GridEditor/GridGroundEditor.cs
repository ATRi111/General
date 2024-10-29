using UnityEditor;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(GridGround))]
    public class GridGroundEditor : AutoEditor
    {
        [AutoProperty]
        public SerializedProperty height, isObstacle;

        protected override void MyOnInspectorGUI()
        {
            isObstacle.BoolField("不可通行");
            if (!isObstacle.boolValue)
            {
                height.IntField("高度");
            }
        }
    }
}