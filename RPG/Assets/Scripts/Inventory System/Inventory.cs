using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    public class Inventory : IInventoryStorage
    {
        public event Action<InventorySlot> OnItemAdded;
        public event Action<InventorySlot> OnItemRemoved;

        private readonly Dictionary<ItemCategory, List<InventorySlot>> items;
        public float CurrentWeight { get; set; }

        public Inventory()
        {
            items = new();
        }

        public void AddItemAndNotify(InventorySlot item)
        {
            if (!AddItem(item)) return;

            OnItemAdded?.Invoke(item);
        }

        public void RemoveItemAndNotify(InventorySlot inventoryItem)
        {
            if (!RemoveItem(inventoryItem)) return;

            OnItemRemoved?.Invoke(inventoryItem);
        }

        public bool AddItem(InventorySlot item)
        {
            if (item == null || item.ItemBase == null || item.Amount <= 0)
                return false;
 
            var itemBase = item.ItemBase;
            var amountToAdd = item.Amount;

            if (!items.TryGetValue(itemBase.Category, out var list))
            {
                list = new List<InventorySlot>();
                items[itemBase.Category] = list;
            }

            if (!itemBase.CanStack)
            {
                list.Add(item);
                Debug.Log($"Added new stack of {itemBase.Name}");
            }
            else
            {
                var remainingAmount = amountToAdd;

                foreach (var stack in list.Where(i => i.ItemBase == itemBase).OrderBy(i => i.Amount))
                {
                    var spaceLeft = itemBase.MaxStackSize - stack.Amount;

                    if (spaceLeft <= 0)
                        continue;

                    var amount = Mathf.Min(spaceLeft, remainingAmount);

                    stack.Amount += amount;
                    Debug.Log($"Added {amount} to existing stack of {itemBase.Name}");
                    remainingAmount -= amount;

                    if (remainingAmount <= 0)
                    {
                        break;
                    }         
                }

                while (remainingAmount > 0)
                {
                    var amount = Mathf.Min(itemBase.MaxStackSize, remainingAmount);

                    list.Add(new InventorySlot(itemBase, amount));
                    Debug.Log($"Added new stack of {itemBase.Name} with amount {amount}");

                    remainingAmount -= amount;
                }
            }

            CurrentWeight += itemBase.Weight * amountToAdd;

            return true;
        }

        public bool RemoveItem(InventorySlot inventoryItem)
        {
            if (inventoryItem == null || inventoryItem.ItemBase == null)
                return false;

            Debug.Log($"Removing {inventoryItem.Amount} of {inventoryItem.ItemBase.Name} from inventory");

            if (!items[inventoryItem.ItemBase.Category].Remove(inventoryItem))
                items[inventoryItem.ItemBase.Category].Remove(FindInventoryItem(inventoryItem.ItemBase, inventoryItem.Amount));

            CurrentWeight -= inventoryItem.ItemBase.Weight * inventoryItem.Amount;

            return true;
        }

        public InventorySlot FindInventoryItem(ItemConfigBase item, int amount = 1)
        {
            if (item == null) return null;
            if (!items.TryGetValue(item.Category, out var list)) return null;

            return list.FirstOrDefault(i => i.ItemBase == item && i.Amount == amount);
        }

        public List<InventorySlot> GetItemsInCategory(ItemCategory category)
        {
            return items.TryGetValue(category, out var itemsList) ? itemsList : new List<InventorySlot>();
        }

        public void Show()
        {
            foreach (var category in items.Keys)
            {
                Debug.Log($"Category: {category}");
                foreach (var item in items[category])
                {
                    Debug.Log($"Item: {item.ItemBase.Name}, Amount: {item.Amount}");
                }
            }
        }
    }
}
