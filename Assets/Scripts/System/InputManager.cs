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
    /// 使用正交相机且视景体沿z轴延伸时，使用此方法获取鼠标位置(Z分量为0)
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public Vector3 MouseWorldPosition2D()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition).ResetZ();
    }

    /// <summary>
    /// 获取鼠标的世界坐标
    /// </summary>
    /// <param name="transform">参照点的世界坐标，过该参照点作平行于成像面的平面，视景体将该平面截成有限平面，假定该平面为屏幕</param>
    /// <param name="camera">以哪个相机的成像为屏幕，默认使用主相机</param>
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
    /// 检测是否连续按下了一串按键
    /// </summary>
    /// <param name="combo">按键序列</param>
    /// <param name="interval">按下两个按键间的最长时间间隔</param>
    public void StartComboCheck(KeyCode[] codes, float interval, Action<bool> callBack)
    {
        
    }
}
