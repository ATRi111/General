using Cinemachine;
using UnityEngine;

namespace Camera
{
    //此脚本添加在VirtualCamera上
    public class CameraCore : MonoBehaviour
    {
        private CinemachineVirtualCamera _VirtualCamera;
        public CinemachineVirtualCamera VirtualCamera
        {
            get
            {
                if (_VirtualCamera == null)
                    Debug.Log("未添加VirtualCamera");
                return _VirtualCamera;
            }
        }
        private CinemachineConfiner _CameraConfiner;
        public CinemachineConfiner CameraConfiner
        {
            get
            {
                if (_CameraConfiner == null)
                    Debug.Log("未添加CameraConfiner");
                return _CameraConfiner;
            }
        }
        private CinemachineCameraOffset _CameraOffset;
        public CinemachineCameraOffset CameraOffset
        {
            get
            {
                if (_CameraOffset == null)
                    Debug.Log("未添加CameraOffset");
                return _CameraOffset;
            }
        }
        public CinemachineImpulseSource _ImpulseSource;
        public CinemachineImpulseSource ImpulseSource
        {
            get
            {
                if (_ImpulseSource == null)
                    Debug.Log("未添加ImpulseSource");
                return _ImpulseSource;
            }
        }
        private CameraMoveController _CameraMoveController;
        public CameraMoveController CameraMoveController
        {
            get
            {
                if (_CameraMoveController == null)
                    Debug.Log("未添加CameraMoveController");
                return _CameraMoveController;
            }
        }

        private void Awake()
        {
            _VirtualCamera = GetComponent<CinemachineVirtualCamera>();
            _CameraConfiner = GetComponent<CinemachineConfiner>();
            _CameraOffset = GetComponent<CinemachineCameraOffset>();
            _ImpulseSource = GetComponent<CinemachineImpulseSource>();
            _CameraMoveController = GetComponent<CameraMoveController>();
        }
    }
}
