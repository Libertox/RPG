using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

namespace InventorySystem
{
    public class Inventory : IInventoryStorage
    {
        public event Action<InventoryItem> OnItemAdded;
        public event Action<ItemConfigBase> OnItemRemoved;

        private readonly Dictionary<ItemCategory, List<InventoryItem>> _items;
        public float CurrentWeight { get; set; }

        public Inventory()
        {
            _items = new();
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

            CurrentWeight += item.Weight * amount;

            Debug.Log($"{item.Name} added");
            return true;
        }

        public bool RemoveItem(ItemConfigBase item)
        {
            var inventoryItem = FindInventoryItem(item);
            if (inventoryItem == null) return false;

            CurrentWeight -= item.Weight;

            _items[item.Category].Remove(inventoryItem);
            Debug.Log($"{item.Name} removed");
            return true;
        }

        private InventoryItem GetOrCreateInventoryItem(ItemConfigBase item)
        {
            if (!_items.TryGetValue(item.Category, out var list))
            {
                list = new List<InventoryItem>();
                _items[item.Category] = list;
            }

            var existing = list.FirstOrDefault(i => i.ItemBase == item);
            if (existing != null) return existing;

            var created = new InventoryItem(item, 0);
            list.Add(created);
            return created;
        }

        public InventoryItem FindInventoryItem(ItemConfigBase item)
        {
            if (item == null) return null;
            if (!_items.TryGetValue(item.Category, out var list)) return null;

            return list.FirstOrDefault(i => i.ItemBase == item);
        }

        public List<InventoryItem> GetItemsInCategory(ItemCategory category)
        {
            return _items.TryGetValue(category, out var itemsList) ? itemsList : new List<InventoryItem>();
        }
    }
}
