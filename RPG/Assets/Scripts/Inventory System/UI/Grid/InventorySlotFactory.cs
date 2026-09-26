
using InventorySystem;
using UnityEngine;

namespace InventorySystem.UI
{
    public class InventorySlotFactory 
    {
        private readonly InventoryItemSlotPool pool;

        public InventorySlotFactory(InventoryItemSlotPool pool)
        {
            this.pool = pool;
        }

        public InventorySlotUI Create(InventorySlot item, RectTransform parent, Vector2 position, Vector2 size)
        {
            return pool.Spawn()
                .Initialize(item)
                .SetParent(parent)
                .SetAnchoredPosition(position)
                .SetSize(size);
        }

        public void Release(InventorySlotUI slot)
        {
            pool.Despawn(slot);
        }

    }
}
