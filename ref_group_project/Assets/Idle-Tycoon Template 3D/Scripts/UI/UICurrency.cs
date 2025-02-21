using TMPro;
using UnityEngine;

namespace IdleTycoon.UI
{
    // UICurrency class handles the Currency UI Element
    public class UICurrency : MonoBehaviour
    {
        // Have a singleton reference for ease of use
        static UICurrency instance;
        public static UICurrency Instance => instance;

        [Tooltip("Container holding all currency Counts")]
        public RectTransform moneyContainer;

        [Tooltip("UI Element for the money count")]
        public TextMeshProUGUI moneyCount;
        public TextMeshProUGUI cornCount;
        public TextMeshProUGUI aplleCount;

        private void Awake()
        {
            // Singletone instantiation
            if (instance == null)
            {
                instance = this;
                return;
            }

            Destroy(this);

            UpdateInitMoneyUI();
        }

        private void Start()
        {
            // Add to OnCurrencyChanged Listener to update Money UI when currency values change.
            CurrencyManager.OnCurrencyChanged.AddListener((CurrencyType type) => UpdateMoneyUI(type));

            // Rebuild UI to ensure its all in the correct place.
            UIUtility.ForceRebuildRecursive(moneyContainer);
        }

        void UpdateInitMoneyUI()
        {
            moneyCount.text = CurrencyManager.Get(CurrencyType.Cash).ToString("0");
            cornCount.text = CurrencyManager.Get(CurrencyType.Corn).ToString("0");
            UIUtility.ForceRebuildRecursive(moneyContainer);
        }

        void UpdateMoneyUI(CurrencyType type)
        {
            // Check the currency type to update
            switch (type)
            {
                case CurrencyType.Cash: 
                    // Update the Cash UI amount based on the current Currency amount available.
                    moneyCount.text = CurrencyManager.Get(CurrencyType.Cash).ToString("0");

                    break;
                case CurrencyType.Corn:
                    cornCount.text = CurrencyManager.Get(CurrencyType.Corn).ToString("0");

                    break;
            }

            // Rebuild UI to ensure its all in the correct place.
            UIUtility.ForceRebuildRecursive(moneyContainer);
        }
    }

}
