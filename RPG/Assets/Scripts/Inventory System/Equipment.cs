using System;
using System.Collections.Generic;

namespace InventorySystem
{
    public class Equipment
    {
        private const int INVALID_SLOT = -1;

        public event Action<ItemInventory, int> OnItemEquipped;
        public event Action<ItemInventory> OnItemUnequipped;

        public event Action<ItemConfigBase, ItemConfigBase> OnItemSwapped;

        private readonly Dictionary<EquipmentSlotCategory, ItemInventory[]> _equipmentItems;

        public Equipment(InventorySettings inventorySettings)
        {
            _equipmentItems = new();

            if (inventorySettings == null) return;

            foreach(var category in inventorySettings.MaxEquipmentSlotsAmount)
            {
                _equipmentItems.Add(category.Key, new ItemInventory[category.Value]);
            }
        }

        public bool TryEquipItem(ItemInventory inventoryItem)
        {
            if (inventoryItem == null || !inventoryItem.ItemBase.CanEquip)
                return false;

            EquipmentSlotCategory category = inventoryItem.ItemBase.EquipmentSlot;
            int slot = GetFreeConsumableSlot(category);

            if (slot == INVALID_SLOT)
                slot = _equipmentItems[category].Length - 1;

            var replaced = _equipmentItems[category][slot];

            if (replaced != null)
                OnItemSwapped?.Invoke(inventoryItem.ItemBase, replaced.ItemBase);

            _equipmentItems[category][slot] = inventoryItem;
            OnItemEquipped?.Invoke(inventoryItem, slot);

            return true;
        }

        private int GetFreeConsumableSlot(EquipmentSlotCategory category)
        {
            for (int i = 0; i < _equipmentItems[category].Length; i++)
            {
                if (_equipmentItems[category][i] == null)
                    return i;
            }

            return INVALID_SLOT;
        }

        public bool TryUnequipItem(EquipmentSlotCategory slotCategory, int slotIndex)
        {
            if (slotIndex < 0) return false;

            var item = _equipmentItems[slotCategory][slotIndex];

            if(item == null) return false;

           OnItemUnequipped?.Invoke(item);
           _equipmentItems[slotCategory][slotIndex] = null;

            return true;
        }

        public ItemInventory GetEquipped(EquipmentSlotCategory slotCategory, int slotIndex)
        {
            return _equipmentItems[slotCategory][slotIndex];
        }

    }
}
