using System.Collections.Generic;

namespace Services.Save
{
    /// <summary>
    /// 管理所有GroupController
    /// </summary>
    public class SaveManagerBase : Service, ISaveManager
    {
        protected Dictionary<int, SaveGroupController> groups = new();

        public SaveGroupController GetGroup(int groupId)
        {
            if (groups.ContainsKey(groupId))
                return groups[groupId];
            Debugger.LogWarning($"{groupId}存档组不存在", EMessageType.Save);
            return null;
        }

        protected override void Awake()
        {
            base.Awake();
            SaveGroupController[] temp = GetComponentsInChildren<SaveGroupController>();
            for (int i = 0; i < temp.Length; i++)
            {
                groups.Add(temp[i].groupId, temp[i]);
            }
        }
    }
}