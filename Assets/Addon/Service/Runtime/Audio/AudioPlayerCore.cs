using UnityEngine;
using UnityEngine.Audio;

namespace Services
{
    public class AudioPlayerCore
    {
        public MyAudioSource CreateAudio(GameObject prefab, Transform parent = null)
        {
            GameObject obj = Object.Instantiate(prefab);
            if (!obj.TryGetComponent(out MyAudioSource myAudioSource))
            {
                Debug.LogWarning("创建的游戏物体未挂载MyAudioSource脚本");
                return null;
            }
            obj.transform.parent = parent;
            obj.transform.position = Vector3.zero;
            return myAudioSource;
        }

        //dB = 10 * lg(p/p0) => dB = k*lg(percent + Δ) + C
        public void SetVolume(AudioMixer mixer, string name, float percent)
        {
            percent = Mathf.Clamp(percent, 0.01f, 1f);
            mixer.SetFloat(name, 40 * Mathf.Log10(percent));
        }
    }
}