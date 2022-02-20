using System.Collections;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public int Size { get; private set; }

    private CObject[] cObjects;     //对象上的脚本

    private int nextIndex;          //下次在对象池中从这个下标开始查找，不完全可靠

    public void Initialize(GameObject sample, int num)
    {
        Size = num;
        cObjects = new CObject[num];
        StartCoroutine(GenerateObject(sample, num));
    }

    private IEnumerator GenerateObject(GameObject sample, int num)
    {
        if (sample.GetComponent<CObject>() == null)
        {
            Debug.LogWarning("对象池中的物体未挂载CObject或其子类的脚本");
            yield break;
        }
        Initializer initializer = Initializer.Instance;
        if (num == 0)
            num = 10;
        initializer.NumOfLoadObject++;
        GameObject temp;
        int count = 0;
        for (; ; )
        {
            //每帧生成10个物体
            for (int i = 0; i < 10; i++)
            {
                temp = Instantiate(sample);
                temp.transform.parent = transform;
                cObjects[count] = temp.GetComponent<CObject>();
                cObjects[count].Initialize();
                count++;
                if (count >= num)
                {
                    initializer.NumOfLoadObject--;
                    yield break;
                }
            }
            yield return null;
        }
    }
    //objectManager以外的类不应该调用这个方法
    public CObject Activate(Vector3 position, float angle)
    {
        if (Size == 0)
            return null;
        //等差数列探测法
        int depth = 0;
        for (int i = nextIndex; i < Size; i++)
        {
            if (!cObjects[i].Active)
            {
                cObjects[i].Activate(position, angle);
                nextIndex = i + depth * 2 + 1;
                return cObjects[i];
            }
            depth++;
        }
        for (int i = 0; i < nextIndex; i++)
        {
            if (!cObjects[i].Active)
            {
                cObjects[i].Activate(position, angle);
                nextIndex = depth * 2 + 1;
                return cObjects[i];
            }
            depth++;
        }
        Debug.LogWarning(gameObject.name + "池中的对象几乎用完了");
        return null;
    }

}