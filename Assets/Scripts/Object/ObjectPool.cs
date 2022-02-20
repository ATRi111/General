using System.Collections;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public int Size { get; private set; }

    private CObject[] cObjects;     //�����ϵĽű�

    private int nextIndex;          //�´��ڶ�����д�����±꿪ʼ���ң�����ȫ�ɿ�

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
            Debug.LogWarning("������е�����δ����CObject��������Ľű�");
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
            //ÿ֡����10������
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
    //objectManager������಻Ӧ�õ����������
    public CObject Activate(Vector3 position, float angle)
    {
        if (Size == 0)
            return null;
        //�Ȳ�����̽�ⷨ
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
        Debug.LogWarning(gameObject.name + "���еĶ��󼸺�������");
        return null;
    }

}