using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

namespace InventorySystem
{
    public class Inventory : IInventoryStorage
    {
        public event Action<ItemInventory> OnItemAdded;
        public event Action<ItemConfigBase> OnItemRemoved;

        private readonly Dictionary<ItemCategory, List<ItemInventory>> _items;
        public ObservableFloat CurrentWeight { get; set; }

        public Inventory()
        {
            _items = new();
            CurrentWeight = new();
        }

        public void AddItemAndNotify(ItemConfigBase item, int amount = 1)
        {
            if (!AddItem(item, amount)) return;
            OnItemAdded?.Invoke(GetOrCreateInventoryItem(item));
        }

        public void RemoveItemAndNotify(ItemConfigBase item)
        {
            if (!RemoveItem(item)) return;
            OnItemRemoved?.Invoke(item);
        }

        public bool AddItem(ItemConfigBase item, int amount = 1)
        {
            if (item == null || amount < 0) return false;

            var inventoryItem = GetOrCreateInventoryItem(item);
            inventoryItem.Amount += amount;

            CurrentWeight.Add(item.Weight * amount);

            Debug.Log($"{item.Name} added");
            return true;
        }

        public bool RemoveItem(ItemConfigBase item)
        {
            var inventoryItem = FindInventoryItem(item);
            if (inventoryItem == null) return false;

            //inventoryItem.Amount--;
            CurrentWeight.Subtract(item.Weight);

            //if (inventoryItem.Amount <= 0)

            _items[item.Category].Remove(inventoryItem);
            Debug.Log($"{item.Name} removed");
            return true;
        }

        private ItemInventory GetOrCreateInventoryItem(ItemConfigBase item)
        {
            if (!_items.TryGetValue(item.Category, out var list))
            {
                list = new List<ItemInventory>();
                _items[item.Category] = list;
            }

            var existing = list.FirstOrDefault(i => i.ItemBase == item);
            if (existing != null) return existing;

            var created = new ItemInventory(item, 0);
            list.Add(created);
            return created;
        }

        public ItemInventory FindInventoryItem(ItemConfigBase item)
        {
            if (item == null) return null;
            if (!_items.TryGetValue(item.Category, out var list)) return null;

            return list.FirstOrDefault(i => i.ItemBase == item);
        }

        public List<ItemInventory> GetItemsInCategory(ItemCategory category)
        {
            return _items.TryGetValue(category, out var itemsList) ? itemsList : new List<ItemInventory>();
        }
    }
}
