using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public class ObjectPool : MonoBehaviour
    {
        private GameObject prefab;
        private List<MyObject> myObjects;   //�����ϵĽű�

        private int nextIndex;              //�´��ڶ�����д�����±꿪ʼ���ң�����ȫ�ɿ�

        internal void Initialize(GameObject sample, int num)
        {
            myObjects = new List<MyObject>(num);
            prefab = sample;
            if (prefab.GetComponent<MyObject>() == null)
            {
                Debug.LogError("������е�����δ����MyObject��������Ľű�");
                return;
            }
            StartCoroutine(GenerateObject(num));
        }

        private IEnumerator GenerateObject(int num)
        {
            Initializer initializer = Initializer.Instance;
            initializer.Count_Initializations++;
            GameObject obj;
            MyObject temp;
            for (; myObjects.Count < num - 10; )
            {
                //ÿ֡����10������
                for (int i = 0; i < 10; i++)
                {
                    obj = Instantiate(prefab);
                    obj.transform.parent = transform;
                    temp = obj.GetComponent<MyObject>();
                    myObjects.Add(temp);
                    temp.Initialize();
                }
                yield return null;
            }
            for (; myObjects.Count < num;)
            {
                obj = Instantiate(prefab);
                obj.transform.parent = transform;
                temp = obj.GetComponent<MyObject>();
                myObjects.Add(temp);
                temp.Initialize();
            }
            initializer.Count_Initializations--;
        }

        internal MyObject Activate(Vector3 position, Vector3 eulerAngles)
        {
            //�Ȳ�����̽�ⷨ
            int depth = 0;
            for (int i = nextIndex; i < myObjects.Count;)
            {
                if (!myObjects[i].Active)
                {
                    myObjects[i].Activate(position, eulerAngles);
                    nextIndex = i + 1;
                    return myObjects[i];
                }
                i += depth * 2 + 1;
                depth++;
            }
            for (int i = 0; i < nextIndex; i++)
            {
                if (!myObjects[i].Active)
                {
                    myObjects[i].Activate(position, eulerAngles);
                    nextIndex = i + 1;
                    return myObjects[i];
                }
                i += depth * 2 + 1;
                depth++;
            }

            Debug.LogWarning(gameObject.name + "���еĶ��󼸺�������");
            GameObject obj;
            MyObject newObject;
            obj = Instantiate(prefab);
            obj.transform.parent = transform;
            newObject = obj.GetComponent<MyObject>();
            myObjects.Add(newObject);
            newObject.Initialize();
            newObject.Activate(position, eulerAngles);
            return newObject;
        }
    }
}