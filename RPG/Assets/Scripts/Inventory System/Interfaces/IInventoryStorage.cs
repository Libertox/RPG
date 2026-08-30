using System;
using System.Collections.Generic;
using Utility;

namespace InventorySystem
{
    public interface IInventoryStorage
    {
        public event Action<InventoryItem> OnItemAdded;
        public event Action<ItemConfigBase> OnItemRemoved;

        public float CurrentWeight { get; set; }

        public void AddItemAndNotify(ItemConfigBase item, int amount = 1);
        public void RemoveItemAndNotify(ItemConfigBase item);
        public bool AddItem(ItemConfigBase item, int amount = 1);
        public bool RemoveItem(ItemConfigBase item);

        public List<InventoryItem> GetItemsInCategory(ItemCategory category);
        public InventoryItem FindInventoryItem(ItemConfigBase item);
    }
}
