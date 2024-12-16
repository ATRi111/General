using System;
using System.Reflection;
using UnityEngine;

namespace Services.Save
{
    /// <summary>
    /// 控制特定对象正确地与SaveData绑定
    /// </summary>
    public class SaveTargetController : MonoBehaviour
    {
        public SaveGroupController Group { get; protected set; }
        [SerializeField]
        protected int groupId;

        [SerializeField]
        protected UnityEngine.Object obj;
        [SerializeField]
        protected string saveDataType;
        [SerializeField]
        protected EIdentifier eIdentifier;
        [SerializeField]
        protected string customIdentifier;
        public virtual string Identifier
        {
            get
            {
                return eIdentifier switch
                {
                    EIdentifier.SceneAndName => SaveUtility.DefineIdentifier_SceneAndName(obj),
                    EIdentifier.NameOnly => obj.name,
                    _ => customIdentifier,
                };
            }
        }

        protected virtual void Awake()
        {
            Group = ServiceLocator.Get<ISaveManager>().GetGroup(groupId);
            Bind(Identifier, obj);
        }

        protected virtual void Bind(string identifier, UnityEngine.Object obj)
        {
            Type type = Type.GetType(saveDataType);
            MethodInfo method = Group.GetType().GetMethod("Bind").MakeGenericMethod(type);
            method.Invoke(Group, new object[] { identifier, obj });
        }
    }

    public enum EIdentifier
    {
        SceneAndName,
        NameOnly,
        Custom,
    }
}