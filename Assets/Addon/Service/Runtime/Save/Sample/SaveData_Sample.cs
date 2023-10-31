namespace Services.Save
{
    public class SaveData_Sample : SaveData
    {
        public static int Id = -114514;
        protected override string Identifier => DefineIdentifier_Default(obj);

        protected override int GroupId => Id;

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
    }
}