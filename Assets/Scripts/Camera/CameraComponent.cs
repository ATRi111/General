using UnityEngine;

public class CameraComponent : MonoBehaviour
{
    protected CameraCore CameraCore { get; private set; }

    //��Ҫ�ڴ˷����л�ȡ����CameraComponent
    protected virtual void Awake()
    {
        CameraCore = GetComponent<CameraCore>();
        if (CameraCore == null)
            Debug.Log("CameraComponentδ��װ��CameraCore��");
    }
}
