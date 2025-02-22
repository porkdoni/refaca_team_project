using UnityEngine;

namespace Quartzified.Audio
{
    public class AudioLayer : MonoBehaviour
    {
        [HideInInspector] public string mixerName = "";

        [Space]

        [SerializeField] AudioSource audioSource;
        public AudioSource Source => audioSource;
        public void SetSource(AudioSource _source) => audioSource = _source;

        [SerializeField] SourceProfile sourceProfile;
        public SourceProfile Profile => sourceProfile;
        public void SetProfile(SourceProfile _profile) => sourceProfile = _profile;

        #region Play OneShot

        public void PlayOneShot(AudioClip clip, bool rndPitch = false)
        {
            if(Source == null) return;

            if(sourceProfile != null)
            {
                // Save Pitch incase we change
                float tempPitch = Source.pitch;

                // Set Pitch if random
                if (rndPitch)
                    Source.pitch = UnityEngine.Random.Range(Profile.LowPitch, Profile.HighPitch);

                Source.PlayOneShot(clip);

                // Reset Pitch
                Source.pitch = tempPitch;
            }
            else
            {
                Source.PlayOneShot(clip);
            }
        }

        public void PlayOneShot(AudioPack pack, bool rndPitch = false)
        {
            if (Source == null) return;

            if (sourceProfile != null)
            {
                // Save Pitch incase we change
                float tempPitch = Source.pitch;

                // Set Pitch if random
                if (rndPitch)
                    Source.pitch = UnityEngine.Random.Range(Profile.LowPitch, Profile.HighPitch);

                // Play Effect
                Source.PlayOneShot(pack.AudioClips[0]);

                // Reset Pitch
                Source.pitch = tempPitch;
            }
            else
            {
                Source.PlayOneShot(pack.AudioClips[0]);
            }
        }

        public void PlayOneShotRandom(AudioClip[] clips, bool rndPitch = false)
        {
            if (Source == null) return;

            if (sourceProfile != null)
            {
                // Save Pitch incase we change
                float tempPitch = Source.pitch;

                // Set Pitch if random
                if (rndPitch)
                    Source.pitch = UnityEngine.Random.Range(Profile.LowPitch, Profile.HighPitch);

                // Select Random Effect
                int rndIndex = UnityEngine.Random.Range(0, clips.Length);

                // Play Random Effect
                Source.PlayOneShot(clips[rndIndex]);

                // Reset Pitch
                Source.pitch = tempPitch;
            }
            else
            {
                int rndIndex = UnityEngine.Random.Range(0, clips.Length);
                Source.PlayOneShot(clips[rndIndex]);
            }
        }

        public void PlayOneShotRandom(AudioPack pack, bool rndPitch = false)
        {
            if (Source == null) return;

            if (sourceProfile != null)
            {
                // Save Pitch incase we change
                float tempPitch = Source.pitch;

                // Set Pitch if random
                if (rndPitch)
                    Source.pitch = UnityEngine.Random.Range(Profile.LowPitch, Profile.HighPitch);

                // Play Random Effect
                Source.PlayOneShot(pack.GetRandomClip());

                // Reset Pitch
                Source.pitch = tempPitch;
            }
            else
            {
                Source.PlayOneShot(pack.GetRandomClip());
            }
        }

        #endregion

        #region Play Audio

        public void PlayAudio(AudioClip clip)
        {
            if (Source == null) return;

            Source.clip = clip;
            Source.Play();
        }

        public void PlayRandomAudio(AudioClip[] clips)
        {
            if (Source == null) return;

            int rndIndex = UnityEngine.Random.Range(0, clips.Length);

            Source.clip = clips[rndIndex];
            Source.Play();
        }

        #endregion
    }
}