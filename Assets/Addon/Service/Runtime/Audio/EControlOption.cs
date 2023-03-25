namespace Services.Audio
{
    /// <summary>
    /// 控制音频自毁的选项
    /// </summary>
    public enum EControlOption
    {
        /// <summary>
        /// 不控制,需要手动摧毁
        /// </summary>
        NoControl,
        /// <summary>
        /// 经过固定时间后自毁
        /// </summary>
        LifeSpan,
        /// <summary>
        /// 播放到指定时间后自毁，可中止
        /// </summary>
        SelfDestructive,
    }
}