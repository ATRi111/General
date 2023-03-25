using UnityEngine;
using UnityEngine.Audio;

namespace Services.Audio
{
    public interface IAudioPlayer : IService
    {
        /// <summary>
        /// 查找带有AudioSource的Prefab，并克隆，以播放音频
        /// </summary>
        /// <param name="identifier">标识符</param>
        /// <param name="position">创建出的游戏物体所处的位置</param>
        /// <param name="parent">创建出的游戏物体挂载的父物体</param>
        /// <param name="option">自毁选项</param>
        /// <param name="lifeSpan">存在时间，opition为EControlOption.NoControl时，此参效无效；为0表示取AudioClip的长度</param>
        AudioSource CreateAudioByPrefab(string identifier, Vector3 position, Transform parent = null, EControlOption option = EControlOption.SelfDestructive, float lifeSpan = 0f);

        /// <summary>
        /// 查找AudioClip，并通过其创建带有AudioSource的游戏物体，以播放音频
        /// </summary>
        /// <param name="identifier">标识符</param>
        /// <param name="position">创建出的游戏物体所处的位置</param>
        /// <param name="parent">创建出的游戏物体挂载的父物体</param>
        /// <param name="option">自毁选项</param>
        /// <param name="lifeSpan">存在时间，opition为EControlOption.NoControl时，此参效无效；为0表示取AudioClip的长度</param>
        AudioSource CreateAudioByClip(string identifier, Vector3 position, Transform parent = null, EControlOption option = EControlOption.SelfDestructive, float lifeSpan = 0f);

        /// <summary>
        /// 设定指定AudioMixer的一个音量参数
        /// </summary>
        /// <param name="audioMixer">指定的AudioMixer</param>
        /// <param name="parameter">音量对应的参数的名称</param>
        /// <param name="volume">音量，输入应当在0~1内，0对应-80dB，1对应+0dB，变化比较平滑</param>
        void SetVolume(AudioMixer audioMixer, string parameter, float volume);
    }
}
