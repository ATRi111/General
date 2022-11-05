using System;

namespace XNodeExtend
{
    [AttributeUsage(AttributeTargets.Method)]
    public class OutputMethodAttribute : Attribute
    {
        public string fieldName;

        /// <param name="fieldName">�˷������ĸ�Output�ֶε����</param>
        public OutputMethodAttribute(string fieldName)
        {
            this.fieldName = fieldName;
        }
    }
}