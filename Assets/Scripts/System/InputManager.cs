using System;
using UnityEngine;

public class InputManager : Service
{
    private Camera mainCamera;
    private EventSystem eventSystem;
    
    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
        eventSystem = Get<EventSystem>();
    }

    private void Start()
    {
        eventSystem.AddListener<int>(EEvent.AfterLoadScene, AfterLoadScene);
    }

    private void AfterLoadScene(int index)
    {
        mainCamera = Camera.main;
    }

    /// <summary>
    /// ʹ������������Ӿ�����z������ʱ��ʹ�ô˷�����ȡ���λ��(Z����Ϊ0)
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public Vector3 MouseWorldPosition2D()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition).ResetZ();
    }

    /// <summary>
    /// ��ȡ������������
    /// </summary>
    /// <param name="transform">���յ���������꣬���ò��յ���ƽ���ڳ������ƽ�棬�Ӿ��彫��ƽ��س�����ƽ�棬�ٶ���ƽ��Ϊ��Ļ</param>
    /// <param name="camera">���ĸ�����ĳ���Ϊ��Ļ��Ĭ��ʹ�������</param>
    /// <returns></returns>
    public Vector3 MouseWorldPosition(Vector3 point, Camera camera = null)
    {
        if (camera == null)
            camera = mainCamera;
        float distance = transform == null ? 0 : camera.transform.InverseTransformPoint(point).z;
        Vector3 mousePosition = Input.mousePosition.ResetZ(distance);
        return camera.ScreenToWorldPoint(mousePosition);
    }

    /// <summary>
    /// ����Ƿ�����������һ������
    /// </summary>
    /// <param name="combo">��������</param>
    /// <param name="interval">����������������ʱ����</param>
    public void StartComboCheck(KeyCode[] codes, float interval, Action<bool> callBack)
    {
        
    }
}
