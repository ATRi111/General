using System;
using System.Reflection;
using UnityEngine;

namespace Services.Save
{
    /// <summary>
    /// 控制特定对象正确地与SaveData绑定
    /// </summary>
    public sealed class SaveTargetController : MonoBehaviour
    {
        public SaveGroupController Group { get; private set; }
        [SerializeField]
        private int groupId;

        [SerializeField]
        private UnityEngine.Object obj;
        [SerializeField]
        private string saveDataType;
        [SerializeField]
        private EIdentifier eIdentifier;
        [SerializeField]
        private string customIdentifier;
        public string Identifier
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

        private void Start()
        {
            Group = ServiceLocator.Get<ISaveManager>().GetGroup(groupId);
            Bind(Identifier, obj);
        }

        private void Bind(string identifier, UnityEngine.Object obj)
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