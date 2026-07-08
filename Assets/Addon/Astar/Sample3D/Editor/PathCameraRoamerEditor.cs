using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace AStar.Sample
{
    [CustomEditor(typeof(PathCameraRoamer))]
    public class PathCameraRoamerEditor : AutoEditor
    {
        [AutoProperty]
        public SerializedProperty sample, flightSpeed,
            shoulderBackDistance, shoulderUpDistance, shoulderSideDistance, positionSmoothSpeed, rotationSmoothSpeed;
        public PathCameraRoamer roamer;

        protected override void OnEnable()
        {
            base.OnEnable();
            roamer = target as PathCameraRoamer;
        }

        protected override void MyOnInspectorGUI()
        {
            sample.PropertyField("寻路组件");
            flightSpeed.FloatField("飞行速度");
            shoulderBackDistance.FloatField("越肩-后方距离");
            shoulderUpDistance.FloatField("越肩-上方距离");
            shoulderSideDistance.FloatField("越肩-侧向距离");
            positionSmoothSpeed.FloatField("位置平滑速度");
            rotationSmoothSpeed.FloatField("朝向平滑速度");
            if (Application.isPlaying)
            {
                if (GUILayout.Button("沿路径飞行"))
                    roamer.FlyCameraAlongPath();
            }
        }
    }
}
