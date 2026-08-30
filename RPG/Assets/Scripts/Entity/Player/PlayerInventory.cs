using System;
using Utility;

namespace InventorySystem
{
    public class PlayerInventory
    {
        public event Action<InventoryItem> OnItemDropped;

        private readonly IInventoryStorage inventoryStorage;
        private readonly Equipment equipment;
        private readonly Wallet wallet;

        public IInventoryStorage InventoryStorage => inventoryStorage;
        public Equipment Equipment => equipment;
        public Wallet Wallet => wallet;
        public float Weight => inventoryStorage.CurrentWeight + equipment.CurrentWeight;

        public PlayerInventory(InventorySettings inventorySettings)
        {
            equipment = new(inventorySettings);
            inventoryStorage = new Inventory();
            wallet = new();

            equipment.OnItemSwapped += OnItemSwapped;
            equipment.OnItemEquipped += OnItemEquipped;
        }

        private void OnItemEquipped(InventoryItem item, int slot)
        {
            InventoryStorage.RemoveItemAndNotify(item.ItemBase);
        }

        private void OnItemSwapped(InventoryItem newItem, InventoryItem oldItem)
        {
            InventoryStorage.AddItem(oldItem.ItemBase, oldItem.Amount);
        }

        public void DropItem(InventoryItem item)
        {
            OnItemDropped?.Invoke(item);

            InventoryStorage.RemoveItemAndNotify(item.ItemBase);
        }
    }

    public class InventoryItem
    {
        public ItemConfigBase ItemBase { get; }
        public int Amount;

        public InventoryItem(ItemConfigBase itemBase, int amount)
        {
            ItemBase = itemBase;
            Amount = amount;
        }
    }
}
