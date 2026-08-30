using TMPro;
using UI;
using UnityEngine;
using Zenject;

namespace InventorySystem.UI
{
    public class CurrencyVisualizer : UIVisualizer
    {
        [SerializeField] private TextMeshProUGUI goldValue;
        [SerializeField] private Currency currency;

        private PlayerInventory playerInventory;

        [Inject]
        private void Construct(PlayerInventory playerInventory)
        {
            this.playerInventory = playerInventory;
        }

        public override void Refresh()
        {
            UpdateGoldValue(playerInventory.Wallet.GetAmount(currency));
        }

        private void UpdateGoldValue(float value)
        {
            goldValue.SetText(value.ToString());
        }

    }
}
