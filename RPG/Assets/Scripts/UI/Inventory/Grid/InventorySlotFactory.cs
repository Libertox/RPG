
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

        public InventoryItemSlot Create(ItemInventory item, RectTransform parent, Vector2 position, Vector2 size, ItemHolder itemHolder)
        {
            return _pool.Spawn()
                .Initialize(item, itemHolder)
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
