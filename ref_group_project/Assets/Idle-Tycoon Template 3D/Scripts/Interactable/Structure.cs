using TMPro;
using System;
using UnityEngine;

namespace IdleTycoon
{
    public class Structure : Interactable
    {
        [Header("Structure")]
        [Tooltip("The ID used to identify the Structure with. Can be used for Data Storage")]
        public string structureID; // Will auto generate OnValidate if Empty
        [Tooltip("Default UI Element showcasing simplified structure information")]
        public TextMeshProUGUI valueText; // Default UI Element

        [Space]
        [Tooltip("CurrencyType used to unlock the structure")]
        public CurrencyType currencyType; // CurrencyType used to unlock structure
        [Tooltip("Structure unlock cost")]
        public double unlockCost; // Structure unlock cost
        [Tooltip("Determines whether the structure is unlocked by default")]
        protected bool unlocked = false; // Determines whether the structure is unlocked by default

        /// <summary>
        /// Checks whether an UI element has been set or not
        /// </summary>
        protected virtual bool HasUI => valueText != null;

        protected virtual void Start()
        {
            // Check if we already unlocked this structure if not by default
            if(!unlocked)
            {
                // Check if we have already set the Unlock Value in PlayerPrefs
                // If we have, then we can set the Structure to unlocked
                unlocked = PlayerPrefs.GetInt("Unlocked_" + structureID, 0) == 1;

            }

            UpdateUI();
        }

        public override void OnEnterInteract() { }
        public override void OnExitInteract() { }

        public override void Interact()
        {
            // Checks whether the Structure has been unlocked
            if (!unlocked)
            {
                // Checks if we currently have enough currency of type
                if (CurrencyManager.Has(currencyType, unlockCost))
                {
                    // Unlocks the Structure
                    // Uses the currency required to unlock
                    unlocked = true;
                    CurrencyManager.Use(currencyType, unlockCost);

                    // Store that we unlocked this structure
                    PlayerPrefs.SetInt("Unlocked_" + structureID, 1);

                    // Triggers OnUnlock Function
                    OnUnlock();

                    // Update UI as needed
                    UpdateUI();
                }
            }
        }

        /// <summary>
        /// Triggered when the structure is unlocked through purchase
        /// </summary>
        protected virtual void OnUnlock() { }

        /// <summary>
        /// Updates the default UI Element if available
        /// </summary>
        protected virtual void UpdateUI()
        {
            // Check if we currently have a UI element set
            if (!HasUI)
                return;

            // Update the text based on if the structure is unlocked or not.
            // Sets the cost of the structure if locked
            valueText.text = unlocked ? "" : "Unlock Cost\n" + (unlockCost <= 0 ? "Free" : unlockCost.ToString("0"));
        }
        public void RefreshUI() => UpdateUI();

        protected override void OnValidate()
        {
            base.OnValidate();

            // If there is no structureID currently set.
            // Then we want to generate a new ID.
            // We use Guid to ensure uniqueness in IDs.
            // When using the same Prefab, ensure the IDs are unique.
            if(string.IsNullOrWhiteSpace(structureID))
            {
                structureID = Guid.NewGuid().ToString();
            }

        }
    }
}
