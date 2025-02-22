using System;
using UnityEngine;

namespace IdleTycoon
{
    public class GameManager : MonoBehaviour
    {
        [Header("Base Settings")]
        [Tooltip("Set target framerate at start of the Application")]
        public int targetFrameRate = 60;

        private void Awake()
        {
            // Set the target framerate of the Application
            Application.targetFrameRate = targetFrameRate;

            // Set the Money of all stored Currencies
            foreach(CurrencyType currency in Enum.GetValues(typeof(CurrencyType)))
            {
                string currencyName = currency.ToString();

                if(PlayerPrefs.HasKey(currencyName))
                {
                    double savedValue = double.Parse(PlayerPrefs.GetString(currencyName));
                    CurrencyManager.Set(currency, savedValue);
                }
            }

            CurrencyManager.OnCurrencyChanged.AddListener((CurrencyType type) => CurrencyManager.Save(type));
        }

        

        private void OnApplicationQuit()
        {
            // Ensure to save when the Application is Quit
            PlayerPrefs.Save();
        }
    }
}