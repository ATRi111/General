using UnityEngine;

public class ObjectData
{
    public EObject EObject { get; private set; }
    /// <summary>
    /// �������ڶ��������Ҫ��������0��ʾ��ʹ�ö����
    /// </summary>
    public int NumInPool { get; private set; }
    public GameObject Prefab { get; set; }
    public ObjectData(EObject eObject, int num, GameObject prefab = null)
    {
        EObject = eObject;
        NumInPool = num;
        Prefab = prefab;
    }
}
