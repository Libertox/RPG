
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    public class PlayerInventory
    {
        private const int INVALID_SLOT = -1;
        private const int MAX_CONSUMABLE_ITEM = 4;

        public event Action<ItemBase> OnItemAdded;
        public event Action<ItemBase> OnItemRemoved;
        public event Action<ItemInventory> OnItemDropped;

        public event Action<ItemInventory> OnItemEquipped;
        public event Action<ItemInventory, int> OnConsumableEquipped;

        public event Action<ItemBase, ItemBase> OnItemSwapped;

        public event Action<int> OnGoldChanged;

        public float LiftingCapacity { get; private set; }
        public int Gold { get; private set; }
        public Dictionary<ItemCategory, List<ItemInventory>> Items { get; private set; }

        public HashSet<ItemInventory> Equipment { get; private set; }
        public ItemInventory[] Consumables { get; private set; }

        public PlayerInventory()
        {
            Items = new();
            Consumables = new ItemInventory[MAX_CONSUMABLE_ITEM];
            Equipment = new();
        }

        public void AddItemAndUpdateInventory(ItemBase item, int amount = 1)
        {
            if (!AddItem(item, amount)) return;
            OnItemAdded?.Invoke(item);
        }

        public void RemoveItemAndUpdateInventory(ItemBase item)
        {
            if (!RemoveItem(item)) return;
            OnItemRemoved?.Invoke(item);
        }

        public bool AddItem(ItemBase item, int amount = 1)
        {
            if (item == null || amount < 0) return false;

            var inventoryItem = GetOrCreateInventoryItem(item);
            inventoryItem.Amount += amount;

            LiftingCapacity += item.Weight * amount;

            Debug.Log($"{item.Name} added");
            return true;
        }

        public bool RemoveItem(ItemBase item)
        {
            var inventoryItem = FindInventoryItem(item);
            if (inventoryItem == null) return false;

            inventoryItem.Amount--;
            LiftingCapacity = Mathf.Max(0, LiftingCapacity - item.Weight);

            if (inventoryItem.Amount <= 0)
                Items[item.Type].Remove(inventoryItem);

            Debug.Log($"{item.Name} removed");
            return true;
        }

        private ItemInventory GetOrCreateInventoryItem(ItemBase item)
        {
            if (!Items.TryGetValue(item.Type, out var list))
            {
                list = new List<ItemInventory>();
                Items[item.Type] = list;
            }

            var existing = list.FirstOrDefault(i => i.ItemBase == item);
            if (existing != null) return existing;

            var created = new ItemInventory(item, 0);
            list.Add(created);
            return created;
        }

        private ItemInventory FindInventoryItem(ItemBase item)
        {
            if (item == null) return null;
            if (!Items.TryGetValue(item.Type, out var list)) return null;

            return list.FirstOrDefault(i => i.ItemBase == item);
        }

        public ItemInventory GetItemInventory(ItemBase item)
        {
            if (item == null) return null;

            for (int i = 0; i < Items[item.Type].Count; i++)
            {
                if (Items[item.Type][i].ItemBase == item) 
                    return Items[item.Type][i];
            }

            return null;
        }

        public bool TryEquipItem(ItemBase item)
        {
            if (item == null || !item.CanEquip)
                return false;

            return item.EquipmentSlot == EquipmentSlotCategory.Consumable
                ? EquipConsumable(item)
                : EquipEquipment(item);
        }

        private bool EquipEquipment(ItemBase item)
        {
            var itemInventory = FindInventoryItem(item);

            if (itemInventory == null) return false;

            var equipped = Equipment.FirstOrDefault(e => e.ItemBase.EquipmentSlot == item.EquipmentSlot);

            if (equipped != null)
            {
                SwapItems(item, equipped.ItemBase);
                Equipment.Remove(equipped);

            }
            else
            {
                RemoveItemAndUpdateInventory(item);
            }

            Equipment.Add(itemInventory);

            OnItemEquipped?.Invoke(itemInventory);

            return true;
        }

        private void SwapItems(ItemBase newItem, ItemBase oldItem)
        {
            AddItem(oldItem, 1);
            RemoveItem(newItem);
            OnItemSwapped?.Invoke(newItem, oldItem);
        }

        private bool EquipConsumable(ItemBase item)
        {
            var inventoryItem = FindInventoryItem(item);
            if (inventoryItem == null) return false;

            int slot = GetFreeConsumableSlot();
            if (slot == INVALID_SLOT)
                slot = MAX_CONSUMABLE_ITEM - 1;

            var replaced = Consumables[slot];

            if (replaced != null)
                SwapItems(item, replaced.ItemBase);
            else
                RemoveItemAndUpdateInventory(item);

            Consumables[slot] = inventoryItem;
            OnConsumableEquipped?.Invoke(inventoryItem, slot);

            return true;
        }

        private int GetFreeConsumableSlot()
        {
            for(int i = 0; i < MAX_CONSUMABLE_ITEM; i++)
            {
                if (Consumables[i] == null)
                    return i;
            }

            return INVALID_SLOT;
        }

        public bool TryUnequipItem(ItemInventory item)
        {
            if (item == null) return false;

            AddItemAndUpdateInventory(item.ItemBase);

            if (item.ItemBase.EquipmentSlot == EquipmentSlotCategory.Consumable)
            {
                int index = Array.IndexOf(Consumables, item);
                if (index >= 0)
                    Consumables[index] = null;
            }
            else
            {
                Equipment.Remove(item);
            }

            return true;
        }

        public void DropItem(ItemBase item)
        {
            OnItemDropped?.Invoke(GetItemInventory(item));

            RemoveItemAndUpdateInventory(item);
        }

        private void AddLiftingCapacity(float value)
        {
            LiftingCapacity += value;
        }

        private void SubstractLiftingCapacity(float value)
        {
            LiftingCapacity -= value;

            if (LiftingCapacity < 0)
                LiftingCapacity = 0;
        }

        public void AddGold(int amount)
        {
            Gold += amount;

            Debug.Log(amount + "gold added");

            OnGoldChanged?.Invoke(Gold);
        }

        public void RemoveGold(int amount)
        {
            Gold -= amount;

            if(Gold < 0)
                Gold = 0;

            Debug.Log(amount + "gold removed");

            OnGoldChanged?.Invoke(Gold);
        }
    }

    public class ItemInventory
    {
        public ItemBase ItemBase;
        public int Amount;

        public ItemInventory(ItemBase itemBase, int amount)
        {
            ItemBase = itemBase;
            Amount = amount;
        }
    }
}
