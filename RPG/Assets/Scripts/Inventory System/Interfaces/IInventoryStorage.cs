using System;
using System.Collections.Generic;

namespace InventorySystem
{
    public interface IInventoryStorage
    {
        public event Action<InventorySlot> OnItemAdded;
        public event Action<InventorySlot> OnItemRemoved;

        public float CurrentWeight { get; set; }

        public void AddItemAndNotify(InventorySlot item);
        public void RemoveItemAndNotify(InventorySlot item);
        public bool AddItem(InventorySlot item);
        public bool RemoveItem(InventorySlot item);

        public List<InventorySlot> GetItemsInCategory(ItemCategory category);
        public InventorySlot FindInventoryItem(ItemConfigBase item, int amount = 1);
        public void Show();
    }
}
