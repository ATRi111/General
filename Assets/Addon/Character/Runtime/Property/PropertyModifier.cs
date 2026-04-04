namespace Character
{
    /// <summary>
    /// 影响属性的词条
    /// </summary>
    [System.Serializable]
    public struct PropertyModifier
    {
        /// <summary>
        /// 变化量
        /// </summary>
        public float value;
        /// <summary>
        /// 提供"获取属性引用的方法"的SO
        /// </summary>
        public FindPropertySO so;
        /// <summary>
        /// 变化方式
        /// </summary>
        public EModifierBucket bucket;
    }
}

