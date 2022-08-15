using System;
using DataStorage;
using UnityEngine;

namespace Audio
{
    public class AudioController : MonoBehaviour
    {
        private enum AudioControllerType
        {
            MenuAudioSource,
            InGameAudioSource
        }
        
        
        [Tooltip("Audio Source")] [SerializeField] private AudioSource audioSource;
        [Tooltip("Audio Controller Type")] [SerializeField] private AudioControllerType audioControllerType;

        void Update()
        {
            switch (audioControllerType)
            {
                case AudioControllerType.MenuAudioSource:
                    if (GameManager.SoundManager.MainMenuSoundVolume is >= 0 and <= 1)
                        audioSource.volume = GameManager.SoundManager.MainMenuSoundVolume;
                    break;
                case AudioControllerType.InGameAudioSource:
                    if (GameManager.SoundManager.InGameSoundVolume is >= 0 and <= 1)
                        audioSource.volume = GameManager.SoundManager.InGameSoundVolume;
                    break;
            }
        }
    }
}
