using UnityEngine;

public class CameraComponent : MonoBehaviour
{
    protected CameraCore CameraCore { get; private set; }

    //不要在此方法中获取其他CameraComponent
    protected virtual void Awake()
    {
        CameraCore = GetComponent<CameraCore>();
        if (CameraCore == null)
            Debug.Log("CameraComponent未安装在CameraCore上");
    }
}
