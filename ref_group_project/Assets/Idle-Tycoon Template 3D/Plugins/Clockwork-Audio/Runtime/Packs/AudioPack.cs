using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Quartzified.Audio
{
    public abstract partial class AudioPack : ScriptableObject
    {
        [Header("Audio Base")]
        [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>(); // Stores a collection of audio clips.
        public List<AudioClip> AudioClips => audioClips; // Provides access to the audio clips.

        [Space]
        [Tooltip("Reference to an AudioMixerGroup for managing audio mixing.")]
        public AudioMixerGroup mixerGroup;

        [Space]
        [Tooltip("Overrides the default volume level for the audio pack.")]
        public float volumeOverride = 0f;

        /// <summary>
        /// Gets the minimum index for audio clips.
        /// </summary>
        public int MinIndex => 0;
        /// <summary>
        /// Gets the maximum index for audio clips.
        /// </summary>
        public int MaxIndex => AudioClips.Count - 1;
        /// <summary>
        /// Returns the total number of audio clips in the pack.
        /// </summary>
        public int ClipCount => AudioClips.Count;

        /// <summary>
        /// Returns the length og the audio clip at the specified index.
        /// </summary>
        /// <param name="clipIndex"></param>
        /// <returns></returns>
        public float GetClipLength(int clipIndex)
        {
            return AudioClips[clipIndex].length;
        }

        /// <summary>
        /// Retrieves a random audio clip from the pack.
        /// </summary>
        /// <returns></returns>
        public AudioClip GetRandomClip()
        {
            return AudioClips[Random.Range(0, ClipCount)];
        }

        /// <summary>
        /// Returns the index of the audio clip.
        /// If the clip is not available in the list the return will be -1.
        /// </summary>
        /// <param name="audioClip"></param>
        /// <returns></returns>
        public int GetIndexFromClip(AudioClip audioClip)
        {
            if (AudioClips.Contains(audioClip))
            {
                return AudioClips.IndexOf(audioClip);
            }

            return -1;
        }
    }

}

