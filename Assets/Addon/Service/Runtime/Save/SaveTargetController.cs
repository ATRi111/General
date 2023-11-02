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
        protected Object obj;

        [SerializeField]
        protected EIdentifier eIdentifier;
        [SerializeField]
        protected string customizedIdentifier;
        public virtual string Identifier
        {
            get
            {
                return eIdentifier switch
                {
                    EIdentifier.SceneAndName => SaveUtility.DefineIdentifier_SceneAndName(obj),
                    EIdentifier.NameOnly => obj.name,
                    _ => customizedIdentifier,
                };
            }
        }

        protected virtual void Awake()
        {
            Group = ServiceLocator.Get<ISaveManager>().GetGroup(groupId);
            Group.Bind<SaveData_Sample>(Identifier, obj);
        }
    }

    public enum EIdentifier
    {
        SceneAndName,
        NameOnly,
        Customized,
    }
}