using UnityEngine;


//Ϊ�˲��ڳ����л�ʱ�����٣��̳д���Ľű�ͨ������ServiceLocator���ڵ���Ϸ��������������ϣ�
/// <summary>
/// ���񣬴ӷ���λ����ȡ
/// </summary>
public abstract class Service : MonoBehaviour
{
    protected virtual void Awake()
    {
        ServiceLocator.Instance.Register(this);
    }

}
