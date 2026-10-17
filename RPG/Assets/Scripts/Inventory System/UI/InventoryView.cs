using Entity.Player;
using InputSystem;
using UI;
using UnityEngine;
using Zenject;

namespace InventorySystem.UI
{
    public class InventoryView : UIViewBase
    {
        [Header("Refernces")]
        [SerializeField] private InventoryDragController dragController;
        [SerializeField] private InventorySortController sortController;
        [SerializeField] private InventoryCategorySelector inventoryCategorySelector;
        [SerializeField] private UIVisualizer[] inventoryVisualizers;

        [SerializeField] private UIViewSO itemFilteringView;

        private PlayerController playerController;
        private InputManager inputManager;
        private UIViewManager viewManager;

        [Inject]
        public void Construct(PlayerController playerController, InputManager inputManager, UIViewManager viewManager)
        {
            this.playerController = playerController;
            this.inputManager = inputManager;
            this.viewManager = viewManager;
        }

        public override void Initialize()
        {
            inventoryCategorySelector.Initialize();
        }

        private void OnEnable()
        {
            playerController.PlayerInventory.InventoryStorage.OnItemRemoved += OnItemRemovedFromInventory;
            playerController.PlayerInventory.InventoryStorage.OnItemAdded += OnItemAddedToInventory;
            playerController.PlayerInventory.Equipment.OnItemSwapped += OnItemSwapInInventory;
        }


        private void OnDisable()
        {
            playerController.PlayerInventory.InventoryStorage.OnItemRemoved -= OnItemRemovedFromInventory;
            playerController.PlayerInventory.InventoryStorage.OnItemAdded -= OnItemAddedToInventory;
            playerController.PlayerInventory.Equipment.OnItemSwapped -= OnItemSwapInInventory;
        }

        private void OnItemSwapInInventory(InventorySlot currentItem, InventorySlot newItem)
        {
            var grid = inventoryCategorySelector.GetGrid(newItem.ItemConfig.Category);

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

            var grid = inventoryCategorySelector.GetGrid(item.ItemConfig.Category);

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
            inputManager.EnableUIActions(true);

            inputManager.OnCancelPressed += OpenPreviousView;
            inputManager.OnFilterItemsPressed += OpenFilteringView;

            dragController.SubscribeInputEvents();
            sortController.SubscribeInputEvents();
        }

        private void OpenFilteringView()
        {
            _= viewManager.TryOpenView(itemFilteringView, false, false);
        }

 
        public override void UnsubscribeToInputEvents()
        {
            inputManager.EnableUIActions(false);

            inputManager.OnCancelPressed -= OpenPreviousView;
            inputManager.OnFilterItemsPressed -= OpenFilteringView;

            dragController.UnsubscribeInputEvents();
            sortController.UnsubscribeInputEvents();
        }

        private async void OpenPreviousView()
        {
            await viewManager.OpenPreviousView();
        }

        private void RefreshInventory()
        {
            foreach (var visualizer in inventoryVisualizers)
                visualizer.Refresh();
        }
    }
}
