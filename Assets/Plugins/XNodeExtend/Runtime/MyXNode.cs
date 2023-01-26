using System;
using System.Collections.Generic;
using System.Reflection;
using XNode;

namespace XNodeExtend
{
    /// <summary>
    /// 此类的子类中的Output和Input字段应声明为public；
    /// DynamicPortList需要特殊处理
    /// </summary>
    public class MyXNode : Node
    {
        protected List<FieldInfo> inputs = new List<FieldInfo>();
        protected List<FieldInfo> listInputs = new List<FieldInfo>();

        protected Dictionary<string, FieldInfo> outputs = new Dictionary<string, FieldInfo>();
        protected Dictionary<string, MethodInfo> outputMethods = new Dictionary<string, MethodInfo>();

        protected override void Init()
        {
            base.Init();
            FieldInfo[] fields = GetType().GetFields();
            foreach (FieldInfo field in fields)
            {
                InputAttribute inputAttribute = MyXNodeUtility.GetAttribute<InputAttribute>(field);
                OutputAttribute outputAttribute = MyXNodeUtility.GetAttribute<OutputAttribute>(field);
                bool list = typeof(Array).IsAssignableFrom(field.FieldType);
                if (inputAttribute != null && !inputAttribute.dynamicPortList)
                {
                    if (list)
                        listInputs.Add(field);
                    else
                        inputs.Add(field);
                }
                if (outputAttribute != null)
                {
                    outputs.Add(field.Name, field);
                }
            }
            MethodInfo[] methodInfos = GetType().GetMethods();
            foreach (MethodInfo methodInfo in methodInfos)
            {
                OutputMethodAttribute attribute = MyXNodeUtility.GetAttribute<OutputMethodAttribute>(methodInfo);
                if (attribute != null && methodInfo.ReturnType != typeof(void) && methodInfo.GetParameters().Length == 0)
                {
                    outputMethods.Add(attribute.fieldName, methodInfo);
                }
            }
        }

        /// <summary>
        /// 让所有Input字段接收来自连线的输入，一般情况下不需要重写
        /// </summary>
        public virtual void GetAllInput()
        {
            foreach (FieldInfo field in inputs)
            {
                field.SetValue(this, GetInputValue(field.FieldType, field.Name));
            }
            foreach (FieldInfo field in listInputs)
            {
                field.SetValue(this, GetInputValues(field.FieldType, field.Name));
            }
        }

        /// <summary>
        /// 根据端口，返回响应的输出，如果没有特殊情况，不需要重写
        /// </summary>
        public override object GetValue(NodePort port)
        {
            string fieldName = port.fieldName;
            if (outputMethods.ContainsKey(fieldName))
                return outputMethods[fieldName].Invoke(this, null);
            if (outputs.ContainsKey(fieldName))
                return outputs[fieldName].GetValue(this);
            return null;
        }
        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            GetAllInput();
        }
        public override void OnRemoveConnection(NodePort port)
        {
            GetAllInput();
        }

        /// <summary>
        /// 使一个端口接收输入
        /// </summary>
        public object GetInputValue(Type type, string fieldName)
        {
            MethodInfo method = typeof(Node).GetMethod(nameof(GetInputValue)).MakeGenericMethod(type);
            object ret = method.Invoke(this, new object[] { fieldName, null });
            return ret;
        }

        /// <summary>
        /// 使一个端口接收输入（用于数组等）
        /// </summary>
        public object GetInputValues(Type arrayType, string fieldName)
        {
            MethodInfo method = typeof(Node).GetMethod(nameof(GetInputValues)).MakeGenericMethod(arrayType.GetElementType());
            object ret = method.Invoke(this, new object[] { fieldName, null });
            return ret;
        }
    }
}