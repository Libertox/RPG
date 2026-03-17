
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class PlayerInventory
    {
        public event Action<ItemBase> OnItemAdded;
        public event Action<ItemBase> OnItemRemoved;

        public event Action<ItemInventory> OnItemEquiped;

        public event Action<int> OnGoldChanged;

        public float LiftingCapacity { get; private set; }
        public int Gold { get; private set; }
        public Dictionary<ItemCategory, List<ItemInventory>> Items { get; private set; }

        public HashSet<ItemInventory> Equipment { get; private set; }
        public List<ItemInventory> Consumable { get; private set; }

        public PlayerInventory()
        {
            Items = new();
            Consumable = new();
            Equipment = new();
        }

        public void AddItem(ItemBase item, int amount = 1)
        {
            if (item == null) return;

            if(!Items.ContainsKey(item.Type))
                Items.Add(item.Type, new());

            if (!ContainItem(item))
                Items[item.Type].Add(new ItemInventory(item, amount));
            else
                GetItemInventory(item).Amount += amount;

            Debug.Log(item.Name + " added to inventory");

            AddLiftingCapacity(item.Weight);

            OnItemAdded?.Invoke(item);
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

            OnItemRemoved?.Invoke(item);
        }

        public ItemInventory GetItemInventory(ItemBase item)
        {
            if (item == null) return null;

            for (int i = 0; i < Items[item.Type].Count; i++)
            {
                if (Items[item.Type][i].ItemBase == item) return Items[item.Type][i];
            }

            return null;
        }
        private bool ContainItem(ItemBase item)
        {
            if (item == null) return false;

            for (int i = 0; i < Items[item.Type].Count; i++)
            {
                if (Items[item.Type][i].ItemBase == item) return true;
            }

            return false;
        }

        public bool TryEquipItem(ItemBase item)
        {
            if (!item.CanEquip) return false;

            var itemInventory = GetItemInventory(item);

            if (itemInventory == null) return false;

            Equipment.Add(itemInventory);

            OnItemEquiped?.Invoke(itemInventory);

            RemoveItem(item);

            return true;
        }

        public bool TryUnequipItem(ItemInventory item)
        {
            if(item == null) return false;

            AddItem(item.ItemBase);

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
