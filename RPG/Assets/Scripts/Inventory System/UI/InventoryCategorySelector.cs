using System;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Zenject;

namespace InventorySystem.UI
{
    public class InventoryCategorySelector : UIVisualizer
    {
        public event Action<ItemCategory> OnCategoryChanged;

        [Header("Refernces")]
        [SerializeField] private ItemCategory defaultCategorySelected;
        [SerializeField] private InventoryItemSlot inventoryItemSlotPrefab;
        [SerializeField] private SerializableDictionary<ItemCategory, InventoryGrid> itemGrids;
        [SerializeField] private ScrollRect scrollArea;
        [SerializeField] private ItemCategoryButton[] itemCategoryButtons;

        private ItemCategory selectedItemCategory;
        private PlayerInventory playerInventory;

        [Inject]
        private void Construct(PlayerInventory playerInventory)
        {
            this.playerInventory = playerInventory;
        }

        public void Initialize()
        {
            SetSelectedItemCategory(defaultCategorySelected);

            foreach (var categoryButton in itemCategoryButtons)
            {
                categoryButton.OnItemCategorySelected += SetSelectedItemCategory;
            }

            foreach (var item in itemGrids.Values)
            {
                item.gameObject.SetActive(false);
            }
        }

        public InventoryGrid GetGrid(ItemCategory category)
        {
            itemGrids.TryGetValue(category, out var grid);
            return grid;
        }

        public override void Refresh()
        {
            GetGrid(selectedItemCategory).GenerateItemSlots(playerInventory.InventoryStorage.GetItemsInCategory(selectedItemCategory));
        }

        private void SetSelectedItemCategory(ItemCategory category)
        {
            if (selectedItemCategory == category) return;

            ToggleCategory(selectedItemCategory, false);

            selectedItemCategory = category;

            ToggleCategory(selectedItemCategory, true);

            if (itemGrids.TryGetValue(category, out var grid))
            {
                scrollArea.content = grid.SlotsContainer;

                Refresh();

                OnCategoryChanged?.Invoke(category);
            }
        }

        private void ToggleCategory(ItemCategory category, bool state)
        {
            if (category == null) return;

            if (itemGrids.TryGetValue(category, out var grid))
            {
                grid.gameObject.SetActive(state);
            }
        }


    }
}
