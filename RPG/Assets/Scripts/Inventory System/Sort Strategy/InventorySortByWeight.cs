

using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Inventory Sort By Weight", menuName = "ScriptableObjects/Inventory System/Sort Strategy/Inventory Sort By Weight")]
    public class InventorySortByWeight : InventorySortStrategy
    {
        public override void Sort(List<InventorySlot> inventorySlots)
        {
            inventorySlots.Sort((a, b) => (b.Amount * b.ItemConfig.Weight).CompareTo(a.Amount * a.ItemConfig.Weight));
        }
    }
}
