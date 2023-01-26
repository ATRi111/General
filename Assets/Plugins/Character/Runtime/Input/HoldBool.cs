namespace Character
{
    /// <summary>
    /// 一旦被设为true，会保持在true，直到将其置为false
    /// </summary>
    public class HoldBool
    {
        private bool boolValue;
        /// <summary>
        /// 从set访问器输入的false值会被忽略
        /// </summary>
        public bool BoolValue
        {
            get => boolValue;
            set
            {
                if (value)
                    boolValue = true;
            }
        }

        public HoldBool(bool init)
        {
            boolValue = init;
        }

        /// <summary>
        /// 只有通过此方法才能将boolValue设为false
        /// </summary>
        public void Reset()
        {
            boolValue = false;
        }

        public void SetTrue()
        {
            BoolValue = true;
        }

        public static bool operator true(HoldBool holdBool) => holdBool.boolValue;
        public static bool operator false(HoldBool holdBool) => !holdBool.boolValue;
    }
}