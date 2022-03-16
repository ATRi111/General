using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public class ObjectPool : MonoBehaviour
    {
        private GameObject prefab;
        private Queue<MyObject> myObjects;  //�����ϵĽű�

        internal void Initialize(GameObject _prefab, int _size)
        {
            myObjects = new Queue<MyObject>(_size);
            prefab = _prefab;
            if (prefab.GetComponent<MyObject>() == null)
            {
                Debug.LogError("������е�����δ����MyObject��������Ľű�");
                return;
            }
            StartCoroutine(GenerateObject(_size));
        }

        private IEnumerator GenerateObject(int num)
        {
            Initializer initializer = Initializer.Instance;
            initializer.Count_Initializations++;
            MyObject temp;
            for (; myObjects.Count < num - 10;)
            {
                //ÿ֡����10������
                for (int i = 0; i < 10; i++)
                {
                    temp = MyObject.Create(prefab, true, this);
                    temp.transform.parent = transform;
                    myObjects.Enqueue(temp);
                }
                yield return null;
            }
            for (; myObjects.Count < num;)
            {
                temp = MyObject.Create(prefab, true, this);
                temp.transform.parent = transform;
                myObjects.Enqueue(temp);
            }
            initializer.Count_Initializations--;
        }

        internal MyObject Activate(Vector3 position, Vector3 eulerAngles)
        {
            MyObject ret;
            if (myObjects.Count > 0)
            {
                ret = myObjects.Dequeue();
                ret.Activate(position, eulerAngles);
            }
            else
            {
                Debug.LogWarning(gameObject.name + "�еĶ���������");
                ret = MyObject.Create(prefab, true, this);
                ret.transform.parent = transform;
                ret.Activate(position, eulerAngles);
            }
            return ret;
        }

        internal void Recycle(MyObject myObject)
        {
            myObjects.Enqueue(myObject);
        }
    }
}