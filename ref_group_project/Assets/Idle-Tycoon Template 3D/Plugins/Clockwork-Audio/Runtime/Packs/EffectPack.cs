using UnityEngine;

namespace Quartzified.Audio
{
    [CreateAssetMenu(menuName = "Quartzified/Audio/Effect Pack", fileName = "New Effect Pack")]
    public class EffectPack : AudioPack
    {
        /// <summary>
        /// Plays the audio effect at the specified index.
        /// </summary>
        /// <param name="index">Index of the audio clip to play. Default is 0.</param>
        public void Play(int index = 0) => AudioManager.Instance.PlayEffect(this, index);

        /// <summary>
        /// Plays a random audio effect from this pack.
        /// </summary>
        public void PlayRandom() => AudioManager.Instance.PlayRandomEffect(this);
    }
}