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

        [Header("Item List Parameters")]
        [SerializeField] private InventoryGridConfig gridConfig;

        private PlayerController _playerController;
        private InputManager _inputManager;
        private UIViewManager _viewManager;

        private ItemCategory _selectedItemCategory;

        private InventoryItemSlotPool _inventoryItemSlotFactory;

        [Inject]
        public void Construct(PlayerController playerController, InputManager inputManager, UIViewManager viewManager, InventoryItemSlotPool inventoryItemSlotFactory)
        {
            _playerController = playerController;
            _inputManager = inputManager;
            _viewManager = viewManager;
            _inventoryItemSlotFactory = inventoryItemSlotFactory;
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

        private void OnItemSwapInInventory(ItemConfigBase currentItem, ItemConfigBase newItem)
        {
            itemGrids[newItem.Category].SetItemOnItemSlot(currentItem, newItem);
        }

        private void OnItemAddedToInventory(ItemConfigBase item)
        {
            itemGrids[item.Category].AddItemToGrid(item);
        }

        private void OnItemRemovedFromInventory(ItemConfigBase item)
        {
            itemGrids[item.Category].RemoveItemFromGrid(item);
        }

        private void OnDisable()
        {
            _playerController.PlayerInventory.InventoryStorage.OnItemRemoved -= OnItemRemovedFromInventory;
            _playerController.PlayerInventory.InventoryStorage.OnItemAdded -= OnItemAddedToInventory;
            _playerController.PlayerInventory.Equipment.OnItemSwapped -= OnItemSwapInInventory;
        }

        private void SetSelectedItemCategory(ItemCategory itemCategory)
        {
            EnableItemSlots(false);

            _selectedItemCategory = itemCategory;
            scrollArea.content = itemGrids[_selectedItemCategory].SlotsContainer;

            EnableItemSlots(true);

            GenerateInvetory(_selectedItemCategory);
        }

        private void EnableItemSlots(bool enable = true)
        {
            if (_selectedItemCategory == null) return;

            itemGrids[_selectedItemCategory].gameObject.SetActive(enable);
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

            GenerateInvetory(_selectedItemCategory);
        }

        private void UpdateGoldValue(int value)
        {
            goldValue.SetText(value.ToString());
        }

        private void UpdateLiftingCapacityValue(float value, float maxValue)
        {
            liftingCapacityValue.SetText($"{value}/{maxValue}");
        }

        private void GenerateInvetory(ItemCategory category)
        {
            itemGrids[category].GenerateItemSlots(_playerController.PlayerInventory.InventoryStorage.GetItemsInCategory(category));
        }

    }
}
