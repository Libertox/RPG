using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class Wallet
    {
        public event Action<Currency, float> OnCurrencyChanged;

        private readonly Dictionary<Currency, float> currencyContainer = new();


        public float GetAmount(Currency currency)
        {
            currencyContainer.TryGetValue(currency, out float amount);
            return amount;
        }

        public void Add(Currency currency, float value)
        {
            if (!currencyContainer.ContainsKey(currency))
                currencyContainer.Add(currency, value);

            currencyContainer[currency] += value;

            OnCurrencyChanged?.Invoke(currency, currencyContainer[currency]);
        }

        public void Remove(Currency currency, float value)
        {
            if (!currencyContainer.ContainsKey(currency))
                currencyContainer.Add(currency, 0);

            currencyContainer[currency] -= value;
            currencyContainer[currency] = Mathf.Clamp(currencyContainer[currency], 0, currencyContainer[currency]);

            OnCurrencyChanged?.Invoke(currency, currencyContainer[currency]);
        }
    }
}
