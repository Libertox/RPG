using Entity.Player;
using InputSystem;
using System;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace InventorySystem.UI
{
    public class InventoryView : UIViewBase
    {
        [Header("Refernces")]
        [SerializeField] private InventoryCategorySelector inventoryCategorySelector;
        [SerializeField] private UIVisualizer[] inventoryVisualizers;

        private PlayerController _playerController;
        private InputManager _inputManager;
        private UIViewManager _viewManager;

        [Inject]
        public void Construct(PlayerController playerController, InputManager inputManager, UIViewManager viewManager)
        {
            _playerController = playerController;
            _inputManager = inputManager;
            _viewManager = viewManager;
        }

        public override void Initialize()
        {
            inventoryCategorySelector.Initialize();
        }

        private void OnEnable()
        {
            _playerController.PlayerInventory.InventoryStorage.OnItemRemoved += OnItemRemovedFromInventory;
            _playerController.PlayerInventory.InventoryStorage.OnItemAdded += OnItemAddedToInventory;
            _playerController.PlayerInventory.Equipment.OnItemSwapped += OnItemSwapInInventory;
        }


        private void OnDisable()
        {
            _playerController.PlayerInventory.InventoryStorage.OnItemRemoved -= OnItemRemovedFromInventory;
            _playerController.PlayerInventory.InventoryStorage.OnItemAdded -= OnItemAddedToInventory;
            _playerController.PlayerInventory.Equipment.OnItemSwapped -= OnItemSwapInInventory;
        }

  
        private void OnItemSwapInInventory(InventorySlot currentItem, InventorySlot newItem)
        {
            var grid = inventoryCategorySelector.GetGrid(newItem.ItemBase.Category);

            if (grid == null) return;

            grid.ReplaceItem(currentItem, newItem);
        }

        private void OnItemAddedToInventory(InventorySlot item)
        {
            RefreshInventory();
        }

        private void OnItemRemovedFromInventory(InventorySlot item)
        {
            if (item == null) return;

            var grid = inventoryCategorySelector.GetGrid(item.ItemBase.Category);

            if (grid == null) return;

            grid.RemoveItemFromGrid(item);
        }

        public override void Open()
        {
            base.Open();

            RefreshInventory();
        }

        public override void SubscribeToInputEvents()
        {
            _inputManager.EnableUIMap(true);

            _inputManager.OnCancelUIButtonPressed += OpenPreviousView;
        }

        public override void UnsubscribeToInputEvents()
        {
            _inputManager.EnableUIMap(false);

            _inputManager.OnCancelUIButtonPressed -= OpenPreviousView;
        }

        private async void OpenPreviousView()
        {
            await _viewManager.OpenPreviousView();
        }

        private void RefreshInventory()
        {
            foreach (var visualizer in inventoryVisualizers)
                visualizer.Refresh();
        }
    }
}
