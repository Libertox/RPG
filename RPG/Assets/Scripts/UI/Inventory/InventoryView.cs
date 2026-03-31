using Entity.Player;
using InputSystem;
using InventorySystem;
using System;
using System.Collections.Generic;
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

        private PlayerData _playerData;
        private InputManager _inputManager;
        private UIViewManager _viewManager;

        private ItemCategory _selectedItemCategory;

        private InventoryItemSlotPool _inventoryItemSlotFactory;

        [Inject]
        public void Construct(PlayerController playerController, InputManager inputManager, UIViewManager viewManager, InventoryItemSlotPool inventoryItemSlotFactory)
        {
            _playerData = playerController.PlayerData;
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
            _playerData.Inventory.OnItemRemoved += OnItemRemovedFromInventory;
            _playerData.Inventory.OnItemAdded += OnItemAddedToInventory;
            _playerData.Inventory.OnItemSwapped += OnItemSwapInInventory;
        }

        private void OnItemSwapInInventory(ItemConfigBase newItem, ItemConfigBase lastItem)
        {
            itemGrids[newItem.Category].SetItemOnItemSlot(lastItem, newItem);
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
            _playerData.Inventory.OnItemRemoved -= OnItemRemovedFromInventory;
            _playerData.Inventory.OnItemAdded -= OnItemAddedToInventory;
            _playerData.Inventory.OnItemSwapped -= OnItemSwapInInventory;
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
            UpdateGoldValue(_playerData.Inventory.Gold);
            UpdateLiftingCapacityValue(_playerData.Inventory.LiftingCapacity, _playerData.MaxLiftingCapacity);

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
            if (!_playerData.Inventory.Items.ContainsKey(category)) return;

            itemGrids[category].GenerateItemSlots(_playerData.Inventory.Items[category]);
        }

    }
}
