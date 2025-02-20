using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace Quartzified.Audio
{
    public class AudioManager : MonoBehaviour
    {
        static AudioManager instance; // Singleton instance of AudioManager.
        public static AudioManager Instance => instance;

        [Header("Effect Settings")]
        [Tooltip("Prefab used for creating audio sources for effects.")]
        public GameObject effectSourcePrefab = null;

        List<AudioSource> effectSourcePool = new List<AudioSource>(); // Pool of audio sources for playing effects.
        int currentEffectSourceIndex = -1; // index of the currently used audio source.

        // Returns the current effect source, or null if unavailable.
        AudioSource currentEffectSource => (currentEffectSourceIndex < 0 || effectSourcePool.Count <= 0 || effectSourcePool.Count < currentEffectSourceIndex) ? null : effectSourcePool[currentEffectSourceIndex];

        private void Awake()
        {
            instance = instance ?? this;
            if(instance != this) 
            {
                DestroyImmediate(this);
                return;
            }

            DontDestroyOnLoad(this.gameObject);
        }

        #region Effect Manager

        /// <summary>
        /// Plays the specified audio effect from an EffectPack at a given index.
        /// </summary>
        /// <param name="effectPack"></param>
        /// <param name="index"></param>
        public void PlayEffect(EffectPack effectPack, int index = 0)
        {
            if (effectPack.ClipCount <= 0 || index > effectPack.MaxIndex)
                return;

            if(currentEffectSourceIndex  == -1 || currentEffectSource.isPlaying)
                currentEffectSourceIndex = GetNextEffectLayerIndex();

            AudioSource playSource = currentEffectSource;
            if(effectPack.mixerGroup != null)
                playSource.outputAudioMixerGroup = effectPack.mixerGroup;

            AudioClip playClip = effectPack.AudioClips[index];

            if (effectPack.volumeOverride > 0)
                playSource.volume = effectPack.volumeOverride;
            else
                playSource.volume = 1;

            playSource.clip = playClip;
            playSource.Play();
        }

        /// <summary>
        /// Plays a random audi effect from an EffectPack.
        /// </summary>
        /// <param name="effectPack"></param>
        public void PlayRandomEffect(EffectPack effectPack)
        {
            if (effectPack.ClipCount <= 0)
                return;

            if (currentEffectSourceIndex == -1 || currentEffectSource.isPlaying)
                currentEffectSourceIndex = GetNextEffectLayerIndex();

            AudioSource playSource = currentEffectSource;
            if (effectPack.mixerGroup != null)
                playSource.outputAudioMixerGroup = effectPack.mixerGroup;

            AudioClip playClip = effectPack.GetRandomClip();

            if (effectPack.volumeOverride > 0)
                playSource.volume = effectPack.volumeOverride;
            else
                playSource.volume = 1;

            playSource.clip = playClip;
            playSource.Play();
        }

        // Gets the index for the next available effect audio source.
        // If non found, we create one.
        int GetNextEffectLayerIndex()
        {
            AudioSource next = effectSourcePool.Find(layer => !layer.isPlaying);
            if(next == null || effectSourcePool.Count <= 0)
            {
                next = Instantiate(effectSourcePrefab, transform).GetComponent<AudioSource>();
                effectSourcePool.Add(next);
            }
            next.gameObject.SetActive(true);
            return effectSourcePool.IndexOf(next);
        }

        #endregion


#if UNITY_EDITOR

        [MenuItem("GameObject/Audio/Audio Manager", false, 0)]
        static void CreateAudioManager(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Audio Manager", typeof(AudioManager));
        }

#endif
    }

}

