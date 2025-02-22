using UnityEngine;

namespace Quartzified.Audio
{
    [CreateAssetMenu(menuName = "Quartzified/Audio/Source Profile", fileName = "New Source Profile")]
    public class SourceProfile : ScriptableObject
    {
        // Random Pitch related settings
        [Tooltip("Can be used for randomized pitch")]
        [SerializeField] Vector2 pitchRange = new Vector2(1f, 1f);
        public Vector2 PitchRange => pitchRange;
        public float LowPitch => PitchRange.x;
        public float HighPitch => PitchRange.y;
    }
}