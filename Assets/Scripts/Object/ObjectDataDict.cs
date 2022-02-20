using System.Collections.Generic;
using UnityEngine;

public enum EObject
{

}

[CreateAssetMenu]
public class ObjectDataDict : ScriptableObject, IInitialize
{
    private readonly Dictionary<EObject, ObjectData> objectDict = new Dictionary<EObject, ObjectData>();

    [Header("˳�������EObjectһ��")]
    [SerializeField]
    private GameObject[] prefabs;
    [Header("˳�������EObjectһ��")]
    [SerializeField]
    private int[] nums;

    public int NumOfObjects { get; private set; }   //��ObjectManager��������Ϸ��������

    public void Initialize()
    {
        ObjectData data;
        NumOfObjects = nums.Length;
        for (int i = 0; i < NumOfObjects; i++)
        {
            data = new ObjectData((EObject)i, nums[i], prefabs[i]);
            objectDict.Add((EObject)i, data);
        }
    }

    public ObjectData GetObject(EObject eObject)
    {
        return objectDict[eObject];
    }
}