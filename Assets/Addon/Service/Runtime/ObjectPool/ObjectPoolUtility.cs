using UnityEngine;

namespace Services
{
    public static class ObjectPoolUtility
    {
        /// <summary>
        /// �ô˷�������MyObject���ص���Ϸ���壬������Object.Instantiate
        /// </summary>
        /// <param name="prefab">Ҫ��¡����Ϸ����</param>
        /// <param name="byPool">�Ƿ��ɶ��������</param>
        /// <param name="pool">�����Ķ����</param>
        /// <returns>���ɵ���Ϸ�����MyObject�ű�</returns>
        public static MyObject Clone(GameObject prefab, bool byPool = false, ObjectPool pool = null)
        {
            GameObject obj = Object.Instantiate(prefab);
            MyObject myObject = obj.GetComponent<MyObject>();
            myObject.b_createdByPool = byPool;
            if (byPool)
            {
                myObject.active = true;
                myObject.Active = false;
                myObject.objectPoolAttached = pool;
            }
            return myObject;
        }

        /// <summary>
        /// ����һ����Ϸ���������������е�MyObject���
        /// </summary>
        public static void RecycleMyObjects(GameObject gameObject)
        {
            MyObject[] myObjects = gameObject.GetComponentsInChildren<MyObject>();
            foreach (MyObject obj in myObjects)
            {
                obj.Recycle();
            }
        }
    }
}