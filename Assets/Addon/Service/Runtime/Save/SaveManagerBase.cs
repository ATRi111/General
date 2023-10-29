using System.Collections.Generic;

namespace Services.Save
{
    /// <summary>
    /// 管理所有GroupController
    /// </summary>
    public class SaveManagerBase : Service, ISaveManager
    {
        protected Dictionary<int, GroupController> groups = new Dictionary<int, GroupController>();

        public GroupController Get(int groupId)
        {
            if (groups.ContainsKey(groupId))
                return groups[groupId];
            return null;
        }

        protected internal override void Init()
        {
            base.Init();

        }
    }
}