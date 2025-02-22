using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IdleTycoon
{
    public static class CurrencyManager
    {
        public static UnityEvent<CurrencyType> OnCurrencyChanged = new UnityEvent<CurrencyType>();

        static Dictionary<CurrencyType, double> currency = new Dictionary<CurrencyType, double>();
        public static Dictionary<CurrencyType, double> Currency => currency;

        /// <summary>
        /// Returns the given CurrencyType amount available.
        /// </summary>
        /// <param name="currencyType"></param>
        /// <returns></returns>
        public static double Get(CurrencyType currencyType)
        {
            if(currency.ContainsKey(currencyType))
                return currency[currencyType];

            return 0;
        }

        /// <summary>
        /// Sets the given CurrenyType available to the given amount.
        /// </summary>
        /// <param name="currencyType"></param>
        /// <param name="amount"></param>
        public static void Set(CurrencyType currencyType, double amount)
        {
            if (currency.ContainsKey(currencyType))
                currency[currencyType] = amount;
            else
                currency[currencyType] = amount;

            OnCurrencyChanged?.Invoke(currencyType);
        }

        /// <summary>
        /// Adds the amount to the given CurrencyType
        /// </summary>
        /// <param name="currencyType"></param>
        /// <param name="amount"></param>
        public static void Add(CurrencyType currencyType, double amount)
        {
            if(currency.ContainsKey(currencyType))
                currency[currencyType] += amount;
            else
                currency[currencyType] = amount;

            OnCurrencyChanged?.Invoke(currencyType);
        }

        /// <summary>
        /// Subtracts / Uses the given amount from the CurrencyType
        /// </summary>
        /// <param name="currencyType"></param>
        /// <param name="amount"></param>
        public static void Use(CurrencyType currencyType, double amount)
        {
            if(currency.ContainsKey(currencyType))
            {
                currency[currencyType] -= amount;
                OnCurrencyChanged?.Invoke(currencyType);
            }
        }

        /// <summary>
        /// Returns whether the given amount is currently available for the CurrencyType
        /// </summary>
        /// <param name="currencyType"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static bool Has(CurrencyType currencyType, double amount)
        {
            if(currency.ContainsKey(currencyType))
                return currency[currencyType] >= amount;

            return amount <= 0;
        }

        public static void Save(CurrencyType currencyType)
        {
            if(currency.ContainsKey(currencyType))
            {
                // Store currency as string, because a double can be greater than int or float.
                PlayerPrefs.SetString(currencyType.ToString(), currency[currencyType].ToString());
            }
        }
    }
}

