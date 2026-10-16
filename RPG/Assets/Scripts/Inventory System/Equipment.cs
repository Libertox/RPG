using Entity.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using InventorySystem.StatisticModifiers;

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

            foreach (var category in inventorySettings.MaxEquipmentSlotsAmount)
            {
                equipmentItems.Add(category.Key, new InventorySlot[category.Value]);
            }
        }

        public bool TryEquipItem(InventorySlot inventoryItem)
        {
            if (inventoryItem == null || !inventoryItem.ItemConfig.CanEquip)
                return false;

            EquipmentSlotCategory category = inventoryItem.ItemConfig.EquipmentSlot;
            int slot = GetEmptySlotIndex(category);

            return TryEquipItem(inventoryItem, slot);
        }

        public bool TryEquipItem(InventorySlot inventoryItem, int slot)
        {
            if (inventoryItem == null || !inventoryItem.ItemConfig.CanEquip)
                return false;

            if (inventoryItem.ItemConfig.RequiredLevel > playerController.Statistic.Level.Value)
                return false;

            EquipmentSlotCategory category = inventoryItem.ItemConfig.EquipmentSlot;

            if (slot == INVALID_SLOT)
                slot = equipmentItems[category].Length - 1;

            var replaced = equipmentItems[category][slot];

            if (replaced != null)
            {
                replaced.RevertAllModifiers(playerController.Statistic);

                OnItemSwapped?.Invoke(inventoryItem, replaced);
                inventoryStorage.RemoveItem(inventoryItem);
                inventoryStorage.AddItem(replaced);
                CurrentWeight -= replaced.ItemConfig.Weight;
            }

            if (replaced == null)
            {
                inventoryStorage.RemoveItemAndNotify(inventoryItem);
            }

            CurrentWeight += inventoryItem.ItemConfig.Weight;
            equipmentItems[category][slot] = inventoryItem;

            inventoryItem.ApplyAllModifiers(playerController.Statistic);

            OnItemEquipped?.Invoke(inventoryItem, slot);

            RefreshItemModifiers();

            return true;
        }

        private int GetEmptySlotIndex(EquipmentSlotCategory category)
        {
            for (int i = 0; i < equipmentItems[category].Length; i++)
            {
                if (equipmentItems[category][i] == null)
                    return i;
            }

            return INVALID_SLOT;
        }

        private void RefreshItemModifiers()
        {
            List<StatisticModifier> statisticModifiers = new();

            foreach (var item in equipmentItems)
            {
                for (int i = 0; i < item.Value.Length; i++)
                {
                    if (item.Value[i] == null) continue;

                    item.Value[i].RevertAllModifiers(playerController.Statistic);

                    if (item.Value[i].MainModifier != null)
                        statisticModifiers.Add(item.Value[i].MainModifier);

                    statisticModifiers.AddRange(item.Value[i].AllModifiers);
                }            
            }

            statisticModifiers = statisticModifiers.OrderByDescending(modifier => modifier.Config.ModifierType).ToList();

            foreach (var modifier in statisticModifiers)
            {
                modifier.Apply(playerController.Statistic);
            }
                      
        }

        public bool TryUnequipItem(EquipmentSlotCategory slotCategory, int slotIndex)
        {
            if (slotIndex < 0) return false;

            var item = equipmentItems[slotCategory][slotIndex];

            if (item == null) return false;

            CurrentWeight -= item.ItemConfig.Weight;

            item.RevertAllModifiers(playerController.Statistic);

            OnItemUnequipped?.Invoke(item);
            equipmentItems[slotCategory][slotIndex] = null;

            RefreshItemModifiers();

            return true;
        }

        public InventorySlot GetEquipped(EquipmentSlotCategory slotCategory, int slotIndex)
        {
            return equipmentItems[slotCategory][slotIndex];
        }

        public bool IsEquipmentSlotEmpty(EquipmentSlotCategory slotCategory, int slotIndex)
        {
            return equipmentItems[slotCategory][slotIndex] == null;
        }
    }
}
