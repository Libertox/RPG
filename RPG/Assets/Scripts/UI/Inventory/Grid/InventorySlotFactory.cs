
using InventorySystem;
using UnityEngine;

namespace UI.Inventory
{
    public class InventorySlotFactory 
    {
        private readonly InventoryItemSlotPool _pool;

        public InventorySlotFactory(InventoryItemSlotPool pool)
        {
            _pool = pool;
        }

        public InventoryItemSlot Create(ItemConfigBase item, RectTransform parent, Vector2 position, Vector2 size)
        {
            return _pool.Spawn()
                .Initialize(item)
                .SetParent(parent)
                .SetAnchoredPosition(position)
                .SetSize(size);
        }

        public void Release(InventoryItemSlot slot)
        {
            _pool.Despawn(slot);
        }

    }
}
