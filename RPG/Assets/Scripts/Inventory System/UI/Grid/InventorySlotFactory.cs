
using InventorySystem;
using UnityEngine;

namespace InventorySystem.UI
{
    public class InventorySlotFactory 
    {
        private readonly InventoryItemSlotPool _pool;

        public InventorySlotFactory(InventoryItemSlotPool pool)
        {
            _pool = pool;
        }

        public InventorySlotUI Create(InventorySlot item, RectTransform parent, Vector2 position, Vector2 size)
        {
            return _pool.Spawn()
                .Initialize(item)
                .SetParent(parent)
                .SetAnchoredPosition(position)
                .SetSize(size);
        }

        public void Release(InventorySlotUI slot)
        {
            _pool.Despawn(slot);
        }

    }
}
