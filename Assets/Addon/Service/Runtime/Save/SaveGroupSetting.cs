using UnityEngine;

namespace Services.Save
{
    /// <summary>
    /// 规定存档组的数量及名称
    /// </summary>
    [CreateAssetMenu]
    public class SaveGroupSetting : ScriptableObject
    {
        public string[] groupNames;
    }
}