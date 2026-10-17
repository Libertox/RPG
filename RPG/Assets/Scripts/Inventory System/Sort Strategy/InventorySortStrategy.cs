using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public abstract class InventorySortStrategy : ScriptableObject
    {
        public abstract void Sort(List<InventorySlot> inventorySlots);
    }
}
