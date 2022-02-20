using UnityEngine;

public class ObjectData
{
    public EObject EObject { get; private set; }
    /// <summary>
    /// 此物体在对象池中需要的数量，0表示不使用对象池
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
