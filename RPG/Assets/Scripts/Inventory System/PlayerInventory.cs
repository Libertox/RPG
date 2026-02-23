
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class PlayerInventory
    {
        public event Action<ItemBase> OnItemAdded;
        public event Action<ItemBase> OnItemRemoved;

        public int LiftingCapacity { get; private set; }
        public int Gold { get; private set; }
        public Dictionary<ItemType, Dictionary<ItemBase, int>> Items { get; private set; }

        public PlayerInventory()
        {
            Items = new();
        }

        public void AddItem(ItemBase item)
        {
            if (item == null) return;

            if(!Items.ContainsKey(item.Type))
                Items.Add(item.Type, new Dictionary<ItemBase, int>());

            if (!Items[item.Type].ContainsKey(item))
                Items[item.Type].Add(item, 1);
            else
                Items[item.Type][item]++;

            Debug.Log(item.Name + " added to inventory");

            LiftingCapacity += item.Weight;

            OnItemAdded?.Invoke(item);
        }

        public void RemoveItem(ItemBase item)
        {
            if (item == null) return;

            if (!Items.ContainsKey(item.Type) || !Items[item.Type].ContainsKey(item))
                return;

            LiftingCapacity -= item.Weight;

            Items[item.Type][item]--;

            if (Items[item.Type][item] <= 0)
                Items[item.Type].Remove(item);

            Debug.Log(item.Name + " removed from inventory");

            OnItemRemoved?.Invoke(item);
        }


    }
}
