using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class CameraImpulseLauncher : CameraComponent
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
