using UnityEditor;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(IsometricObjectBrush))]
    public class IsometricObjectBrushEditor : ObjectBrushEditor
    {
        [AutoProperty]
        public SerializedProperty lockLayer, layer, lockXY;

        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
            lockLayer.BoolField("锁定层数");
            if(lockLayer.boolValue)
            {
                layer.IntField("层数");
            }
            lockXY.BoolField("锁定XY");
        }

        protected override void OnKeyDown(KeyCode keyCode)
        {
            currentEvent.Use();
            if (keyCode == KeyCode.LeftControl || keyCode == KeyCode.RightControl)
            {
                lockXY.boolValue = true;
            }
            else if (keyCode >= KeyCode.Alpha0 && keyCode <= KeyCode.Alpha9)
            {
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
                lockLayer.boolValue = !lockLayer.boolValue;
            }
        }
        protected override void OnKeyUp(KeyCode keyCode)
        {
            if (keyCode == KeyCode.LeftControl || keyCode == KeyCode.RightControl)
                lockXY.boolValue = false;
        }
    }
}