using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Inventory Sort By Price", menuName = "ScriptableObjects/Inventory System/Sort Strategy/Inventory Sort By Price")]
    public class InventorySortByPrice : InventorySortStrategy
    {
        public override void Sort(List<InventorySlot> inventorySlots)
        {
            inventorySlots.Sort((a, b) => (b.Amount * b.ItemConfig.Gold).CompareTo(a.ItemConfig.Gold * a.Amount));
        }
    }
}
