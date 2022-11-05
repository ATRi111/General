using System;

namespace XNodeExtend
{
    [AttributeUsage(AttributeTargets.Method)]
    public class OutputMethodAttribute : Attribute
    {
        public string fieldName;

        /// <param name="fieldName">此方法是哪个Output字段的输出</param>
        public OutputMethodAttribute(string fieldName)
        {
            this.fieldName = fieldName;
        }
    }
}