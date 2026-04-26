using Entity.Player;
using InputSystem;
using InventorySystem;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Zenject;

namespace UI.Inventory
{
    public class InventoryView : MonoBehaviour, IView
    {
        [Header("Refernces")]
        [SerializeField] private ItemCategory defaultCategorySelected;
        [SerializeField] private InventoryItemSlot inventoryItemSlotPrefab;
        [SerializeField] private SerializableDictionary<ItemCategory, InventoryGrid> itemGrids;
        [SerializeField] private ScrollRect scrollArea;
        [SerializeField] private ItemCategoryButton[] itemCategoryButtons;

        [Header("Statistic Texts")]
        [SerializeField] private TextMeshProUGUI goldValue;
        [SerializeField] private TextMeshProUGUI liftingCapacityValue;

        private PlayerController _playerController;
        private InputManager _inputManager;
        private UIViewManager _viewManager;

        private ItemCategory _selectedItemCategory;

        [Inject]
        public void Construct(PlayerController playerController, InputManager inputManager, UIViewManager viewManager)
        {
            _playerController = playerController;
            _inputManager = inputManager;
            _viewManager = viewManager;
        }

        public void Initialize()
        {
            foreach(var item in itemGrids.Values)
            {
                item.gameObject.SetActive(false);
            }

            SetSelectedItemCategory(defaultCategorySelected);

            foreach(var categoryButton in itemCategoryButtons)
            {
                categoryButton.OnItemCategorySelected += SetSelectedItemCategory;
            }
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

        private void OnItemSwapInInventory(ItemInventory currentItem, ItemInventory newItem)
        {
            var grid = GetInventoryGrid(newItem.ItemBase.Category);

            if (grid == null) return;

            grid.SetItemOnItemSlot(currentItem, newItem);
        }

        private void OnItemAddedToInventory(ItemInventory item)
        {
            var grid = GetInventoryGrid(item.ItemBase.Category);

            if (grid == null) return;

            grid.AddItemToGrid(item);
        }

        private void OnItemRemovedFromInventory(ItemConfigBase item)
        {
            var grid = GetInventoryGrid(item.Category);

            if (grid == null) return;

            grid.RemoveItemFromGrid(item);
        }

        private InventoryGrid GetInventoryGrid(ItemCategory itemCategory)
        {
            itemGrids.TryGetValue(itemCategory, out var grid);

            return grid;
        }

        private void SetSelectedItemCategory(ItemCategory category)
        {
            if (_selectedItemCategory == category) return;

            ToggleCategory(_selectedItemCategory, false);

            _selectedItemCategory = category;

            ToggleCategory(_selectedItemCategory, true);

            if (itemGrids.TryGetValue(category, out var grid))
            {
                scrollArea.content = grid.SlotsContainer;
                GenerateInventory(category);
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

        public void Open()
        {
            gameObject.SetActive(true);

            UpdateInventory();
        }

        public Task OpenAsync()
        {
            Open();

            return Task.CompletedTask;
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public Task CloseAsync()
        {
            Close();

            return Task.CompletedTask;
        }      

        public void SubscribeToInputEvents()
        {
            _inputManager.EnableUIMap(true);

            _inputManager.OnCancelUIButtonPressed += OpenPreviousView;
        }

        public void UnsubscribeToInputEvents()
        {
            _inputManager.EnableUIMap(false);

            _inputManager.OnCancelUIButtonPressed -= OpenPreviousView;
        }

        private async void OpenPreviousView()
        {
            await _viewManager.OpenPreviousView();
        }

        private void UpdateInventory()
        {
            UpdateGoldValue(_playerController.PlayerInventory.Gold);
            UpdateLiftingCapacityValue(_playerController.PlayerInventory.InventoryStorage.CurrentWeight.Value, _playerController.PlayerData.MaxLiftingCapacity);

            GenerateInventory(_selectedItemCategory);
        }

        private void UpdateGoldValue(int value)
        {
            goldValue.SetText(value.ToString());
        }

        private void UpdateLiftingCapacityValue(float value, float maxValue)
        {
            liftingCapacityValue.SetText($"{value}/{maxValue}");
        }

        private void GenerateInventory(ItemCategory category)
        {
            itemGrids[category].GenerateItemSlots(_playerController.PlayerInventory.InventoryStorage.GetItemsInCategory(category));
        }

    }
}
