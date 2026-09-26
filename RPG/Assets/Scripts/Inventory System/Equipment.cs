using Entity.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class Equipment
    {
        private const int INVALID_SLOT = -1;

        public event Action<InventorySlot, int> OnItemEquipped;
        public event Action<InventorySlot> OnItemUnequipped;

        public event Action<InventorySlot, InventorySlot> OnItemSwapped;

        private readonly Dictionary<EquipmentSlotCategory, InventorySlot[]> equipmentItems;
        private readonly IInventoryStorage inventoryStorage;
        private readonly PlayerController playerController;

        public float CurrentWeight { get; private set; }

        public Equipment(InventorySettings inventorySettings, IInventoryStorage inventoryStorage, PlayerController playerController)
        {
            equipmentItems = new();
            this.inventoryStorage = inventoryStorage;
            this.playerController = playerController;

            if (inventorySettings == null) return;

            foreach(var category in inventorySettings.MaxEquipmentSlotsAmount)
            {
                equipmentItems.Add(category.Key, new InventorySlot[category.Value]);
            }
        }

        public bool TryEquipItem(InventorySlot inventoryItem)
        {
            if (inventoryItem == null || !inventoryItem.ItemBase.CanEquip)
                return false;

            EquipmentSlotCategory category = inventoryItem.ItemBase.EquipmentSlot;
            int slot = GetFreeConsumableSlot(category);

            return TryEquipItem(inventoryItem, slot);
        }

        public bool TryEquipItem(InventorySlot inventoryItem, int slot)
        {
            if (inventoryItem == null || !inventoryItem.ItemBase.CanEquip)
                return false;

            if (inventoryItem.ItemBase.RequiredLevel > playerController.Statistic.Level.Value)
                return false;

            EquipmentSlotCategory category = inventoryItem.ItemBase.EquipmentSlot;

            if (slot == INVALID_SLOT)
                slot = equipmentItems[category].Length - 1;

            var replaced = equipmentItems[category][slot];

            if (replaced != null)
            {
                OnItemSwapped?.Invoke(inventoryItem, replaced);
                inventoryStorage.RemoveItem(inventoryItem);
                inventoryStorage.AddItem(replaced);
                CurrentWeight -= replaced.ItemBase.Weight;
            }

            if (replaced == null)
            {
                inventoryStorage.RemoveItemAndNotify(inventoryItem);
            }

            CurrentWeight += inventoryItem.ItemBase.Weight;
            equipmentItems[category][slot] = inventoryItem;
            OnItemEquipped?.Invoke(inventoryItem, slot);

            return true;
        }

        private int GetFreeConsumableSlot(EquipmentSlotCategory category)
        {
            for (int i = 0; i < equipmentItems[category].Length; i++)
            {
                if (equipmentItems[category][i] == null)
                    return i;
            }

            return INVALID_SLOT;
        }

        public bool TryUnequipItem(EquipmentSlotCategory slotCategory, int slotIndex)
        {
            if (slotIndex < 0) return false;

            var item = equipmentItems[slotCategory][slotIndex];

            if(item == null) return false;

            CurrentWeight -= item.ItemBase.Weight;
            OnItemUnequipped?.Invoke(item);
            equipmentItems[slotCategory][slotIndex] = null;

            return true;
        }

        public InventorySlot GetEquipped(EquipmentSlotCategory slotCategory, int slotIndex)
        {
            return equipmentItems[slotCategory][slotIndex];
        }

        public bool IsSlotOccupied(EquipmentSlotCategory slotCategory, int slotIndex)
        {
            return equipmentItems[slotCategory][slotIndex] != null;
        }

    }
}
