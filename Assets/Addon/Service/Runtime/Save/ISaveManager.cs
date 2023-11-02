namespace Services.Save
{
    public interface ISaveManager : IService
    {
        /// <summary>
        /// 获取指定GroupController
        /// </summary>
        SaveGroupController GetGroup(int groupId);
    }
}