using Cinemachine;
using UnityEngine;

namespace MyCamera
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class ImpulseLauncher : CameraComponent
    {
        private CinemachineImpulseSource impulseSource;
        protected override void Awake()
        {
            base.Awake();
            impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        public void Launch()
        {
            impulseSource.GenerateImpulse();
        }
    }
}
