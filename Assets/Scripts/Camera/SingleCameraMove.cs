using UnityEngine;

namespace Camera
{
    [System.Serializable]
    public class SingleCameraMove
    {
        /// <summary>
        /// 用Destination==IDLE表示相机不动（而不是以原点为Destination）
        /// </summary>
        public static Vector3 IDLE = Vector3.zero;

        [SerializeField]
        private float _Duration;
        /// <summary>
        /// 移动持续时间
        /// </summary>
        public float Duration => _Duration;
        [SerializeField]
        private Vector3 _Destination;
        /// <summary>
        /// 目的地
        /// </summary>
        public Vector3 Destination => _Destination;
        public SingleCameraMove(float destination, Vector2 offset)
        {
            _Duration = destination;
            _Destination = offset;
        }

        public static CameraMove operator +(SingleCameraMove s1, SingleCameraMove s2)
        {
            CameraMove c = new CameraMove();
            c.singleCameraMoves.Add(s1);
            if (s2 != null)
                c.singleCameraMoves.Add(s2);
            return c;
        }
    }
}