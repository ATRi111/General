using System.Text;
using UnityEngine;

namespace Services.Save
{
    public class SaveTargetController_Sample : SaveTargetController
    {
        protected override void Bind(string identifier, Object obj)
        {
            Group.Bind<SaveData_Sample>(identifier, obj);
        }
    }

    public class SaveData_Sample : SaveData
    {
        private SampleTarget Sample => obj as SampleTarget;

        public int[] data1;
        public float data2;
        public string data3;

        public override void Load()
        {
            Sample.data1 = data1;
            Sample.data2 = data2;
            Sample.data3 = data3;
        }

        public override void Save()
        {
            data1 = Sample.data1;
            data2 = Sample.data2;
            data3 = Sample.data3;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data1.Length; i++)
            {
                sb.AppendLine(data1[i].ToString());
            }
            sb.AppendLine(data2.ToString());
            sb.AppendLine(data3.ToString());
            return sb.ToString();
        }
    }
}