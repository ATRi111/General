using UnityEditor;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(IsometricObjectBrush))]
    public class IsometricObjectBrushEditor : ObjectBrushEditor
    {
        public new IsometricObjectBrush ObjectBrush => target as IsometricObjectBrush;
        [AutoProperty]
        public SerializedProperty lockLayer, layer;

        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
            lockLayer.BoolField("��������");
            if(lockLayer.boolValue)
            {
                layer.IntField("����");
            }
            EditorGUILayout.HelpBox("��סCtrl����XY;�����ּ��������������", MessageType.Info);
        }

        protected override void OnKeyDown(KeyCode keyCode)
        {
            if (keyCode == KeyCode.LeftControl || keyCode == KeyCode.RightControl)
            {
                ObjectBrush.lockXY = true;
            }
            else if (keyCode >= KeyCode.Alpha0 && keyCode <= KeyCode.Alpha9)
            {
                currentEvent.Use();
                int num = keyCode - KeyCode.Alpha0;
                if (lockLayer.boolValue)
                {
                    if (layer.intValue == num)
                        lockLayer.boolValue = false;
                    else
                        layer.intValue = num;
                }
                else
                {
                    lockLayer.boolValue = true;
                    layer.intValue = num;
                }
            }
            else if(keyCode == KeyCode.BackQuote)
            {
                currentEvent.Use();
                lockLayer.boolValue = !lockLayer.boolValue;
            }
        }
        protected override void OnKeyUp(KeyCode keyCode)
        {
            if (keyCode == KeyCode.LeftControl || keyCode == KeyCode.RightControl)
                ObjectBrush.lockXY = false;
        }
    }
}