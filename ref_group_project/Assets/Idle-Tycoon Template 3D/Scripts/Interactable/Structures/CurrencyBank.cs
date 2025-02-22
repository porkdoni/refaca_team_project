using Quartzified.Audio;
using TMPro;
using UnityEngine;

namespace IdleTycoon
{
    // Currency Bank is a structure which earns currency each print cycle
    public class CurrencyBank : Structure
    {
        [Header("Bank")]
        [Tooltip("Time in seconds untill the next print")]
        public float printFrequency = 1; // Seconds untill next print
        [Tooltip("CurrencyType of the printed Currency")]
        public CurrencyType printType = CurrencyType.Cash; // Print currency Type
        [Tooltip("Currency value gained when print")]
        public double printAmount = 1; // Value gained when print

        // Time untill next print
        float printTime;

        [Header("Visual")]
        [Tooltip("Text Popup element for when the structure prints currency")]
        public GameObject earnEffect;
        [Tooltip("Additionaly offset paramater for earnEffect spawn position")]
        public Vector3 earnEffectOffset;

        [Header("Audio")]
        [Tooltip("AudioPack to play for when you unlock the Structure")]
        public EffectPack unlockEffect;

        void Update()
        {
            // Checks whether the structure has been unlocked
            if (!unlocked)
                return;

            // Checks whether we should try to print
            if (printTime <= 0)
            {
                // Add currency of type and amount
                CurrencyManager.Add(currencyType, printAmount);

                // Show Earn Effect if available
                if (earnEffect != null)
                {
                    // Spawn Earn Effect at the structures position with offset
                    GameObject pop = Instantiate(earnEffect, this.transform.position + earnEffectOffset, Quaternion.identity);
                    // Set the Earn Effects text to the amount gained
                    pop.GetComponentInChildren<TextMeshProUGUI>().text = "+" + printAmount.ToString("0");

                    // Destroy pop after 2 seconds
                    Destroy(pop, 2f);
                }

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
                // Set text to nothing if unlocked
                valueText.text = "";
            }
        }
    }

}
