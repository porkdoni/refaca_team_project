using Quartzified.Audio;
using UnityEngine;

namespace IdleTycoon
{
    // State Structure is a structure which changes state dependent on the unlock state.
    public class StateStructure : Structure
    {
        [Header("States")]
        public GameObject lockedState;
        public GameObject unlockedState;

        [Header("Audio")]
        [Tooltip("AudioPack to play for when you unlock the Structure")]
        public EffectPack unlockEffect;

        protected override void Start()
        {
            // We need to call base.Start() to check if the structure has been unlocked or not
            base.Start();

            // After we checked the initial unlock state we can now call UpdateState();
            UpdateState();
        }

        protected override void UpdateUI()
        {
            // Check if we currently have a UI element set
            if (!HasUI)
                return;

            // Check if our structure is currently locked
            if (!unlocked)
            {
                // Sets the cost of the structure if locked
                valueText.text = "Unlock Cost\n" + (unlockCost <= 0 ? "Free" : unlockCost.ToString("0"));
            }
            else
            {
                // Clears the text if the structure is unlocked
                valueText.text = "";
            }
        }

        protected override void OnUnlock()
        {
            // Play Unlock sound
            unlockEffect.PlayRandom();

            // We unlocked the structure. So now we update the State.
            UpdateState();
        }

        void UpdateState()
        {
            // Set LockedState active if we have not unlocked the structure yet
            lockedState.SetActive(!unlocked);

            // Set UnlockedState active if we have unlocked the structure
            unlockedState.SetActive(unlocked);
        }

    }
}