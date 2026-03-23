
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    public class PlayerInventory
    {
        private const int ALL_CONSUMABLE_SLOT_OCCUPIED = -1;
        private const int MAX_CONSUMABLE_ITEM = 4;

        public event Action<ItemBase> OnItemAdded;
        public event Action<ItemBase> OnItemRemoved;

        public event Action<ItemInventory> OnItemEquiped;
        public event Action<ItemInventory, int> OnConsumableEquiped;

        public event Action<ItemBase, ItemBase> OnItemSwaped;

        public event Action<int> OnGoldChanged;

        public float LiftingCapacity { get; private set; }
        public int Gold { get; private set; }
        public Dictionary<ItemCategory, List<ItemInventory>> Items { get; private set; }

        public HashSet<ItemInventory> Equipment { get; private set; }
        public ItemInventory[] Consumable { get; private set; }

        public PlayerInventory()
        {
            Items = new();
            Consumable = new ItemInventory[MAX_CONSUMABLE_ITEM];
            Equipment = new();
        }

        public void AddItemAndUpdateInventory(ItemBase item, int amount = 1)
        {
            if (item == null) return;

            AddItem(item, amount);

            OnItemAdded?.Invoke(item);
        }

        public void AddItem(ItemBase item, int amount = 1)
        {
            if (item == null) return;

            if (!Items.ContainsKey(item.Type))
                Items.Add(item.Type, new());

            if (!ContainItem(item))
                Items[item.Type].Add(new ItemInventory(item, amount));
            else
                GetItemInventory(item).Amount += amount;

            Debug.Log(item.Name + " added to inventory");

            AddLiftingCapacity(item.Weight);
        }

        public void RemoveItemAndUpdateInventory(ItemBase item)
        {
            if (item == null) return;

            RemoveItem(item);

            OnItemRemoved?.Invoke(item);
        }

        public void RemoveItem(ItemBase item)
        {
            if (item == null) return;

            if (!Items.ContainsKey(item.Type) || !ContainItem(item))
                return;

            SubstractLiftingCapacity(item.Weight);

            ItemInventory itemInventory = GetItemInventory(item);

            itemInventory.Amount--;

            if (itemInventory.Amount <= 0)
                Items[item.Type].Remove(itemInventory);

            Debug.Log(item.Name + " removed from inventory");
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
        private bool ContainItem(ItemBase item)
        {
            if (item == null) return false;

            for (int i = 0; i < Items[item.Type].Count; i++)
            {
                if (Items[item.Type][i].ItemBase == item) 
                    return true;
            }

            return false;
        }

        public bool TryEquipItem(ItemBase item)
        {
            if (!item.CanEquip) return false;

            if (item.EquipmentSlot == EquipmentSlotCategory.Consumable) return TryAddConsumable(item);
            else return TryAddEquipment(item);
        }

        private bool TryAddEquipment(ItemBase item)
        {
            var itemInventory = GetItemInventory(item);

            if (itemInventory == null) return false;

            if (IsCategoryItemEquiped(item.EquipmentSlot))
            {
                var categoryItem = GetEquipedItemByCategory(item.EquipmentSlot);

                AddItem(categoryItem.ItemBase);

                RemoveItem(item);

                Equipment.Remove(categoryItem);

                OnItemSwaped?.Invoke(item, categoryItem.ItemBase);
            }
            else
            {
                RemoveItemAndUpdateInventory(item);
            }

            Equipment.Add(itemInventory);

            OnItemEquiped?.Invoke(itemInventory);

            return true;
        }

        private bool TryAddConsumable(ItemBase item)
        {
            var itemInventory = GetItemInventory(item);

            if (itemInventory == null) return false;

            int targetSlot = GetFreeConsumableItemSlotIndex();

            Debug.Log(targetSlot);

            if (targetSlot == ALL_CONSUMABLE_SLOT_OCCUPIED)
            {
                targetSlot = MAX_CONSUMABLE_ITEM - 1;

                var categoryItem = Consumable[targetSlot];

                AddItem(categoryItem.ItemBase);

                RemoveItem(item);

                OnItemSwaped?.Invoke(item, categoryItem.ItemBase);
            }
            else
            {
                RemoveItemAndUpdateInventory(item);
            }

            Consumable[targetSlot] = itemInventory;

            OnConsumableEquiped?.Invoke(itemInventory, targetSlot);

            return true;
        }

        private int GetFreeConsumableItemSlotIndex()
        {
            for(int i = 0; i < MAX_CONSUMABLE_ITEM; i++)
            {
                if (Consumable[i] == null)
                    return i;
            }

            return ALL_CONSUMABLE_SLOT_OCCUPIED;
        }

        private int GetIndexByConsumable(ItemInventory itemInventory)
        {
            for (int i = 0; i < Consumable.Length; i++)
            {
                if (Consumable[i] == itemInventory)
                    return i;
            }

            return -1;
        }

        public bool IsCategoryItemEquiped(EquipmentSlotCategory category)
        {
            var equipedItem = Equipment.FirstOrDefault((item) => item.ItemBase.EquipmentSlot == category);

            return equipedItem != default;
        }

        public ItemInventory GetEquipedItemByCategory(EquipmentSlotCategory category)
        {
            return Equipment.First((item) => item.ItemBase.EquipmentSlot == category);
        }

        public bool TryUnequipItem(ItemInventory item)
        {
            if(item == null) return false;

            AddItemAndUpdateInventory(item.ItemBase);

            if (item.ItemBase.EquipmentSlot == EquipmentSlotCategory.Consumable)
                Consumable[GetIndexByConsumable(item)] = null;
            else
                Equipment.Remove(item);

            return true;
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
