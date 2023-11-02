using UnityEngine;

namespace Services.Save
{
    /// <summary>
    /// 控制特定对象正确地与SaveData绑定
    /// </summary>
    public class SaveTargetController : MonoBehaviour
    {
        protected SaveGroupController controller;
        [SerializeField]
        protected int groupId;

        protected virtual Object Obj => gameObject;

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
                    EIdentifier.SceneAndName => SaveUtility.DefineIdentifier_SceneAndName(Obj),
                    EIdentifier.NameOnly => Obj.name,
                    _ => customizedIdentifier,
                };
            }
        }

        protected void Awake()
        {
            controller = ServiceLocator.Get<ISaveManager>().GetGroup(groupId);
            controller.Bind<SaveData_Sample>(Identifier, this);
        }
    }

    public enum EIdentifier
    {
        SceneAndName,
        NameOnly,
        Customized,
    }
}