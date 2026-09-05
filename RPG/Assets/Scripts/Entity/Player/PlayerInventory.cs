using System;

namespace InventorySystem
{
    public class PlayerInventory
    {
        public event Action<InventorySlot> OnItemDropped;

        private readonly IInventoryStorage inventoryStorage;
        private readonly Equipment equipment;
        private readonly Wallet wallet;

        public IInventoryStorage InventoryStorage => inventoryStorage;
        public Equipment Equipment => equipment;
        public Wallet Wallet => wallet;
        public float Weight => inventoryStorage.CurrentWeight + equipment.CurrentWeight;

        public PlayerInventory(InventorySettings inventorySettings)
        {
            inventoryStorage = new Inventory();
            equipment = new(inventorySettings, inventoryStorage);    
            wallet = new();
        }

        public void DropItem(InventorySlot item)
        {
            OnItemDropped?.Invoke(item);

            InventoryStorage.RemoveItemAndNotify(item);
        }
    }
}
