using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    public class ObjectPool : MonoBehaviour
    {
        private GameObject prefab;
        private List<MyObject> myObjects;   //对象上的脚本

        private int nextIndex;              //下次在对象池中从这个下标开始查找，不完全可靠

        internal void Initialize(GameObject sample, int num)
        {
            myObjects = new List<MyObject>(num);
            prefab = sample;
            if (prefab.GetComponent<MyObject>() == null)
            {
                Debug.LogError("对象池中的物体未挂载MyObject或其子类的脚本");
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
                //每帧生成10个物体
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
            //等差数列探测法
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

            Debug.LogWarning(gameObject.name + "池中的对象几乎用完了");
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