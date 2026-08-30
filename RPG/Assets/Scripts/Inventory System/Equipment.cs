using System;
using System.Collections.Generic;

namespace InventorySystem
{
    public class Equipment
    {
        private const int INVALID_SLOT = -1;

        public event Action<InventoryItem, int> OnItemEquipped;
        public event Action<InventoryItem> OnItemUnequipped;

        public event Action<InventoryItem, InventoryItem> OnItemSwapped;

        private readonly Dictionary<EquipmentSlotCategory, InventoryItem[]> _equipmentItems;
        public float CurrentWeight { get; private set; }

        public Equipment(InventorySettings inventorySettings)
        {
            _equipmentItems = new();

            if (inventorySettings == null) return;

            foreach(var category in inventorySettings.MaxEquipmentSlotsAmount)
            {
                _equipmentItems.Add(category.Key, new InventoryItem[category.Value]);
            }
        }

        public bool TryEquipItem(InventoryItem inventoryItem)
        {
            if (inventoryItem == null || !inventoryItem.ItemBase.CanEquip)
                return false;

            EquipmentSlotCategory category = inventoryItem.ItemBase.EquipmentSlot;
            int slot = GetFreeConsumableSlot(category);

            return TryEquipItem(inventoryItem, slot);
        }

        public bool TryEquipItem(InventoryItem inventoryItem, int slot)
        {
            if (inventoryItem == null || !inventoryItem.ItemBase.CanEquip)
                return false;

            EquipmentSlotCategory category = inventoryItem.ItemBase.EquipmentSlot;

            if (slot == INVALID_SLOT)
                slot = _equipmentItems[category].Length - 1;

            var replaced = _equipmentItems[category][slot];

            if (replaced != null)
                OnItemSwapped?.Invoke(inventoryItem, replaced);

            _equipmentItems[category][slot] = inventoryItem;
            OnItemEquipped?.Invoke(inventoryItem, slot);

            CurrentWeight += inventoryItem.ItemBase.Weight;

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

            CurrentWeight -= item.ItemBase.Weight;
            OnItemUnequipped?.Invoke(item);
           _equipmentItems[slotCategory][slotIndex] = null;

            return true;
        }

        public InventoryItem GetEquipped(EquipmentSlotCategory slotCategory, int slotIndex)
        {
            return _equipmentItems[slotCategory][slotIndex];
        }

    }
}
