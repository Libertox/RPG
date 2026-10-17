

using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Inventory Sort By Rarity", menuName = "ScriptableObjects/Inventory System/Sort Strategy/Inventory Sort By Rarity")]
    public class InventorySortByRarity : InventorySortStrategy
    {
        public override void Sort(List<InventorySlot> inventorySlots)
        {
            inventorySlots.Sort((a, b) => b.ItemConfig.Rarity.Name.CompareTo(a.ItemConfig.Rarity.Name));
        }
    }
}
