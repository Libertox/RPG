using UnityEngine;

namespace InventorySystem.UI
{
    public class InventoryGridNode
    {
        public Vector2Int GridPosition { get; private set; }
        public Vector2 WorldPosition { get; private set; }
        public InventorySlotUI Slot { get; set; }

        public InventoryGridNode(Vector2Int gridPosition, Vector2 worldPosition, InventorySlotUI slot)
        {
            GridPosition = gridPosition;
            WorldPosition = worldPosition;
            Slot = slot;
        }
    }
}
