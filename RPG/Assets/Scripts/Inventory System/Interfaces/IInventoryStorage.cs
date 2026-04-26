using System;
using System.Collections.Generic;
using Utility;

namespace InventorySystem
{
    public interface IInventoryStorage
    {
        public event Action<ItemInventory> OnItemAdded;
        public event Action<ItemConfigBase> OnItemRemoved;

        public ObservableFloat CurrentWeight { get; set; }

        public void AddItemAndNotify(ItemConfigBase item, int amount = 1);
        public void RemoveItemAndNotify(ItemConfigBase item);
        public bool AddItem(ItemConfigBase item, int amount = 1);
        public bool RemoveItem(ItemConfigBase item);

        public List<ItemInventory> GetItemsInCategory(ItemCategory category);
        public ItemInventory FindInventoryItem(ItemConfigBase item);
    }
}
