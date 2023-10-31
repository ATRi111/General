using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        TD td = new TD();
        ITDS itds = new ITDS();
        Debug.Log(JsonConvert.SerializeObject(td, Formatting.Indented));
        Debug.Log(JsonConvert.SerializeObject(itds, Formatting.Indented));
    }

    [System.Serializable]
    class TD
    {
        public int a;
        public TD()
        {
            a = Random.Range(0, 100);
        }
    }

    [System.Serializable]
    class ITD
    {
        public TD td;
    }

    class ITDS : ITD
    {
        internal List<TD> list;
        public Dictionary<int, TD> dic;

        public ITDS()
        {
            td = new TD();
            list = new List<TD>();
            dic = new Dictionary<int, TD>();
            for (int i = 0; i < 3; i++)
            {
                list.Add(new TD());
                dic.Add(i, new TD());
            }
        }
    }
}