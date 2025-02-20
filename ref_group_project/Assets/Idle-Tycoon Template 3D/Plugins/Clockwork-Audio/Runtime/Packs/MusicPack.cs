using UnityEngine;

namespace Quartzified.Audio
{
    [CreateAssetMenu(menuName = "Quartzified/Audio/Music Pack", fileName = "New Music Pack")]
    public class MusicPack : AudioPack
    {
        [Header("Music Base")]
        [SerializeField] float reverbTail = 0f;
        public float ReverbTail => reverbTail;

    }
}
