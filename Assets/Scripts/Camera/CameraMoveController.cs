using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyCamera
{
    public class CameraMoveController : CameraComponent
    {
        private CinemachineCameraOffset cameraOffset;

        [SerializeField]
        private bool b_2D;  //是否为2D模式，2D模式下相机不会进行Z方向的移动

        protected override void Awake()
        {
            base.Awake();
            cameraOffset = CameraCore.CameraOffset;
        }

        public void StartMove(CameraMove cameraMove)
        {
            StartCoroutine(Move(cameraMove));
        }

        private IEnumerator Move(CameraMove move)
        {
            List<SingleCameraMove> singleCameraMoves = move.singleCameraMoves;

            float timer;
            for (int i = 0; i < singleCameraMoves.Count; i++)
            {
                SingleCameraMove singleCameraMove = singleCameraMoves[i];
                float duration = singleCameraMove.Duration;
                Vector3 deltaOffset;
                if (singleCameraMove.Destination == SingleCameraMove.IDLE)
                {
                    deltaOffset = Vector3.zero;
                }
                else
                {
                    if (duration < 0.1f)
                    {
                        cameraOffset.m_Offset = singleCameraMove.Destination - transform.position - cameraOffset.m_Offset;
                        continue;
                    }
                    deltaOffset = Time.fixedDeltaTime / duration * (singleCameraMove.Destination - transform.position - cameraOffset.m_Offset);
                    if (b_2D)
                        deltaOffset.ResetZ();
                }

                for (timer = 0; timer < duration; timer += Time.fixedDeltaTime)
                {
                    cameraOffset.m_Offset += deltaOffset;
                    yield return new WaitForFixedUpdate();
                }
            }
            cameraOffset.m_Offset = Vector3.zero;
        }
    }
}