using Quartzified.Audio;
using System.Collections.Generic;
using UnityEngine;

namespace IdleTycoon
{
    // Currency Area Printer is a structure which spawn collectables in the surrounding area
    public class CurrencyAreaPrinter : Structure
    {
        [Header("Printer")]
        [Tooltip("Collectable spawned by the printer")]
        public CurrencyCollectable currencyObject; // Collectable Spawned 
        [Tooltip("Radius in which the Printer will place the collectables when spawned")]
        public float printArea; // Print Area
        [Tooltip("Additional offset paramater for placing height")]
        public float yOffset; // Height Offset

        /// <summary>
        /// Gets the random Spawn Location for the collectable to be placed at.
        /// </summary>
        public Vector3 spawnLocation
        {
            get
            {
                Vector3 loc = Random.onUnitSphere * printArea;
                loc += this.transform.position;
                loc.y = this.transform.position.y + yOffset;
                return loc;
            }

        }

        [Space]
        [Tooltip("Time in seconds untill the next print")]
        public float printFrequency = 1; // Seconds untill next print
        [Tooltip("CurrencyType of the printed Collectable")]
        public CurrencyType printType = CurrencyType.Cash; // Print Collectable currency Type
        [Tooltip("Currency Value of the printed Collectable")]
        public double printAmount = 1; // Value given to the printed collectable

        [Space]
        [Tooltip("Max amount of collectables that can be available at once")]
        public double maxPrintAmount = 20;

        [Header("Audio")]
        [Tooltip("AudioPack to play for when you unlock the Structure")]
        public EffectPack unlockEffect;

        // Time untill next print
        float printTime;

        // List of currently active collectables
        List<CurrencyCollectable> collectables = new List<CurrencyCollectable>();

        // Fill amount of active collectables (0f - 1f)
        float filledAmount => (float)(collectables.Count / maxPrintAmount);

        void Update()
        {
            // Checks whether the structure has been unlocked
            if (!unlocked)
                return;

            // Checks whether we should try to print the next Collectable
            if (printTime <= 0)
            {
                // Checks if we have can print more Collectables
                if (collectables.Count < maxPrintAmount)
                {
                    // Create new Collectable
                    // Sets Collectable CurrencyType & Value
                    CurrencyCollectable collectable = Instantiate(currencyObject, spawnLocation, Quaternion.identity);
                    collectable.currencyType = currencyType;
                    collectable.amount = printAmount;

                    // Add the Collectable into the tracking list
                    collectables.Add(collectable);
                }

                // Set next Print frequency
                printTime = printFrequency;
            }
            else // If not ready to print reduce print time
                printTime -= Time.deltaTime;

            // Remove all collectables that are no longer available / have been destroyed
            collectables.RemoveAll(collectable => collectable == null);

            // Update UI
            UpdateUI();
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
                valueText.text = collectables.Count.ToString("0") + "/" + maxPrintAmount.ToString("0");
            }
        }

        protected override void OnUnlock()
        {
            // Set printTime when unlocked to not instantly print the first collectable
            printTime = printFrequency;
            // Play Unlock sound
            unlockEffect.PlayRandom();
        }

    }
}