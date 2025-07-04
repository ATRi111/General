using MyTool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Audio
{
    public enum EPlayMode
    {
        Sequential,
        Loop,
        Random
    }

    [Serializable]
    public class AudioPlayList
    {
        private static IAudioPlayer audioPlayer;
        private static IAudioPlayer AudioPlayer
        {
            get
            {
                if (audioPlayer == null)
                    audioPlayer = ServiceLocator.Get<IAudioPlayer>();
                return audioPlayer;
            }
        }

        public Transform mountPoint;

        public List<GameObject> playList;

        [NonSerialized]
        public SelfDestructiveAudio currentAudio;
        private List<int> sequence = new();
        public int currentSequenceIndex;

        [SerializeField]
        private EPlayMode playMode;
        public EPlayMode PlayMode
        {
            get => playMode;
            set
            {
                if (playMode != value)
                {
                    playMode = value;
                    Reset(value);
                }
            }
        }

        public void Reset(EPlayMode mode)
        {
            //获取当前播放的音频在playList中的序号
            int currentAudioIndex = -1;
            if (currentSequenceIndex >= 0 && currentSequenceIndex < sequence.Count)
                currentAudioIndex = sequence[currentSequenceIndex];
            //重新确定播放顺序
            switch (mode)
            {
                case EPlayMode.Sequential:
                case EPlayMode.Loop:
                    sequence.Clear();
                    for (int i = 0; i < playList.Count; i++)
                    {
                        sequence.Add(i);
                    }
                    break;
                case EPlayMode.Random:
                    sequence = RandomTool.GetPermutation(playList.Count);
                    break;

            }
            //确定当前音频在新的播放列表中的序号
            if (currentAudioIndex != -1)
            {
                for (int i = 0; i < sequence.Count; i++)
                {
                    if (sequence[i] == currentAudioIndex)
                    {
                        currentSequenceIndex = i;
                        break;
                    }
                }
            }
            else
            {
                switch (mode)
                {
                    case EPlayMode.Sequential:
                    case EPlayMode.Random:
                        currentSequenceIndex = -1;
                        break;
                    case EPlayMode.Loop:
                        currentSequenceIndex = 0;
                        break;
                }
            }
        }

        private void PlayNext()
        {
            switch (PlayMode)
            {
                case EPlayMode.Sequential:
                    currentSequenceIndex = (currentSequenceIndex + 1) % sequence.Count;
                    break;
                case EPlayMode.Random:
                    currentSequenceIndex++;
                    if (currentSequenceIndex == sequence.Count)
                    {
                        sequence = RandomTool.GetPermutation(playList.Count);
                        currentSequenceIndex = 0;
                    }
                    break;
            }
            GameObject audioPrefab = playList[sequence[currentSequenceIndex]];
            currentAudio = AudioPlayer.CreateAudioByPrefab(audioPrefab.name, mountPoint.position, mountPoint, EControlOption.SelfDestructive)
                .GetComponent<SelfDestructiveAudio>();
            currentAudio.BeforeDestroy += PlayNext;
        }

        public void Play()
        {
            if (currentAudio != null)
                currentAudio.audioSource.Play();
            else
            {
                Reset(PlayMode);
                PlayNext();
            }
        }

        public void UnPause()
        {
            if (currentAudio != null)
                currentAudio.audioSource.UnPause();
        }

        public void Pause()
        {
            if (currentAudio != null)
                currentAudio.audioSource.Pause();
        }
    }
}