using UnityEngine;
using UnityEngine.Audio;

namespace Services.Audio
{
    public class AudioPlayerCore
    {
        internal AudioSource CreateAudioByPrefab(GameObject prefab, Vector3 position, Transform parent, EControlOption option, float time)
        {
            GameObject obj = Object.Instantiate(prefab);
            obj.transform.SetParent(parent);
            obj.transform.position = position;
            if (obj.TryGetComponent(out AudioSource audioSource))
            {
                ControlDestroy(obj, audioSource, option, time);
                return audioSource;
            }
            return null;
        }

        internal AudioSource CreateAudioByClip(AudioClip clip, Vector3 position, Transform parent, EControlOption option, float time)
        {
            GameObject obj = new GameObject(clip.name);
            obj.transform.SetParent(parent);
            obj.transform.position = position;
            AudioSource audioSource = obj.AddComponent<AudioSource>();
            audioSource.clip = clip;
            ControlDestroy(obj, audioSource, option, time);
            return audioSource;
        }

        private void ControlDestroy(GameObject obj, AudioSource audioSource, EControlOption option, float time)
        {
            if (time == 0f)
                time = audioSource.clip.length;
            switch (option)
            {
                case EControlOption.NoControl:
                    break;
                case EControlOption.LifeSpan:
                    Object.Destroy(obj, time);
                    break;
                case EControlOption.SelfDestructive:
                    SelfDestructiveAudio audio = obj.AddComponent<SelfDestructiveAudio>();
                    audio.timing = time;
                    break;
            }
        }

        //dB = 10 * lg(p/p0) => dB = k*lg(percent + жд) + C
        internal void SetVolume(AudioMixer mixer, string name, float percent)
        {
            percent = Mathf.Clamp(percent, 0.01f, 1f);
            mixer.SetFloat(name, 40 * Mathf.Log10(percent));
        }
    }
}