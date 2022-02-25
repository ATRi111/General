using System.Collections;
using UnityEngine;

namespace ObjectPool
{
    public class ObjectPool : MonoBehaviour
    {
        public int Size { get; private set; }

        private MyObject[] cObjects;     //对象上的脚本

        private int nextIndex;          //下次在对象池中从这个下标开始查找，不完全可靠

        internal void Initialize(GameObject sample, int num)
        {
            Size = num;
            cObjects = new MyObject[num];
            StartCoroutine(GenerateObject(sample, num));
        }

        private IEnumerator GenerateObject(GameObject sample, int num)
        {
            if (sample.GetComponent<MyObject>() == null)
            {
                Debug.LogWarning("对象池中的物体未挂载CObject或其子类的脚本");
                yield break;
            }
            Initializer initializer = Initializer.Instance;
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
                    cObjects[count] = temp.GetComponent<MyObject>();
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

        internal MyObject Activate(Vector3 position, Vector3 eulerAngles)
        {
            if (Size == 0)
                return null;
            //等差数列探测法
            int depth = 0;
            for (int i = nextIndex; i < Size;)
            {
                if (!cObjects[i].Active)
                {
                    cObjects[i].Activate(position, eulerAngles);
                    nextIndex = i + 1;
                    return cObjects[i];
                }
                i += depth * 2 + 1;
                depth++;
            }
            for (int i = 0; i < nextIndex; i++)
            {
                if (!cObjects[i].Active)
                {
                    cObjects[i].Activate(position, eulerAngles);
                    nextIndex = i + 1;
                    return cObjects[i];
                }
                i += depth * 2 + 1;
                depth++;
            }
            Debug.LogWarning(gameObject.name + "池中的对象几乎用完了");
            return null;
        }
    }
}