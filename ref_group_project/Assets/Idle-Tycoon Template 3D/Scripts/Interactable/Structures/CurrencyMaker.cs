    using Quartzified.Audio;
using UnityEngine;

namespace IdleTycoon
{
    // Currency Maker is a structure which stores currency each print cycle
    // Which can be collected on interaction
    public class CurrencyMaker : Structure
    {
        [Header("Maker")]
        [Tooltip("Time in seconds untill the next print")]
        public float printFrequency = 1; // Seconds untill next print
        [Tooltip("CurrencyType of the printed Currency")]
        public CurrencyType printType = CurrencyType.Cash; // Print currency Type
        [Tooltip("Currency value gained when printed")]
        public double printAmount = 1; // Value gained when print

        [Space]
        [Tooltip("Max amount of currency that can be stored at once")]
        public double maxPrintAmount = 60;

        // Time untill next print
        float printTime;
        // Amount of currently stored currency
        [SerializeField] double totalPrintAmount;

        // Fill amount of stored currency (0f - 1f)
        float filledAmount => (float)(totalPrintAmount / maxPrintAmount);

        [Header("Audio")]
        [Tooltip("AudioPack to play for when you unlock the Structure")]
        public EffectPack unlockEffect;
        [Tooltip("AudioPack to play for when you collect from the Structure")]
        public EffectPack collectEffect;

        public override void Interact()
        {
            // Use Base Interact event to enable unlocking of the structure
            base.Interact();

            // Checks whether the structure has been unlocked
            if (!unlocked)
                return;

            // Check if we have more than 0 currency currently stored
            if (totalPrintAmount > 0)
            {
                // Add currency of type and amount
                CurrencyManager.Add(printType, totalPrintAmount);
                // Reset stored currency back to 0
                totalPrintAmount = 0;

                // Play random collect effect from pack
                collectEffect.PlayRandom();

                // Update UI
                UpdateUI();
            }
        }

        void Update()
        {
            // Checks whether the structure has been unlocked
            if (!unlocked)
                return;

            // Checks whether we should try to print
            if (printTime <= 0)
            {
                // Add printAmount to the stored Total
                totalPrintAmount += printAmount;
                // Check if we are over the max stored total
                // Set to the store amount to max store total if over
                if (totalPrintAmount > maxPrintAmount)
                    totalPrintAmount = maxPrintAmount;

                // Update UI
                UpdateUI();

                // Set next Print frequency
                printTime = printFrequency;
            }
            else // If not ready to print reduce print time
                printTime -= Time.deltaTime;

        }

        protected override void OnUnlock()
        {
            // Set printTime when unlocked to not instantly print the first collectable
            printTime = printFrequency;
            // Play Unlock sound
            unlockEffect.PlayRandom();
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
                // Sets the current print fill amount to text
                valueText.text = totalPrintAmount.ToString("0") + "/" + maxPrintAmount.ToString("0");
            }
        }
    }
}