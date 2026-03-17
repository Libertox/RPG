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
        [SerializeField] private InventoryItemSlot inventoryItemSlotPrefab;
        [SerializeField] private SerializableDictionary<ItemCategory, RectTransform> itemSlotsContainer;
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

        private Dictionary<ItemCategory, InventoryGrid> _invetoryGrids;
        private InventoryItemSlotFactory _inventoryItemSlotFactory;

        [Inject]
        public void Construct(PlayerController playerController, InputManager inputManager, UIViewManager viewManager)
        {
            _playerData = playerController.PlayerData;
            _inputManager = inputManager;
            _viewManager = viewManager;
        }

        public void Initialize()
        {
            foreach(var item in itemSlotsContainer.Values)
            {
                item.gameObject.SetActive(false);
            }

            SetSelectedItemCategory(ItemCategory.Weapon);

            foreach(var categoryButton in itemCategoryButtons)
            {
                categoryButton.OnItemCategorySelected += SetSelectedItemCategory;
            }

            _inventoryItemSlotFactory = new(inventoryItemSlotPrefab);
            _invetoryGrids = new();
        }

        private void OnEnable()
        {
            _playerData.Inventory.OnItemRemoved += OnItemRemovedFromInventory;
            _playerData.Inventory.OnItemAdded += OnItemAddedToInventory;
        }

        private void OnItemAddedToInventory(ItemBase item)
        {
            _invetoryGrids[item.Type].AddItemToGrid(item);
        }

        private void OnItemRemovedFromInventory(ItemBase item)
        {
            _invetoryGrids[item.Type].RemoveItemFromGrid(item);
        }

        private void OnDisable()
        {
            _playerData.Inventory.OnItemRemoved -= OnItemRemovedFromInventory;
            _playerData.Inventory.OnItemAdded -= OnItemAddedToInventory;
        }

        private void SetSelectedItemCategory(ItemCategory itemCategory)
        {
            EnableItemSlots(false);

            _selectedItemCategory = itemCategory;
            scrollArea.content = itemSlotsContainer[_selectedItemCategory];

            EnableItemSlots(true);
        }

        private void EnableItemSlots(bool enable = true)
        {
            itemSlotsContainer[_selectedItemCategory].gameObject.SetActive(enable);
        }

        public void Open()
        {
            UpdateInventory();

            gameObject.SetActive(true);
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

            GenerateInvetory();
        }

        private void UpdateGoldValue(int value)
        {
            goldValue.SetText(value.ToString());
        }

        private void UpdateLiftingCapacityValue(float value, float maxValue)
        {
            liftingCapacityValue.SetText($"{value}/{maxValue}");
        }

        private void GenerateInvetory()
        {
            foreach(ItemCategory type in Enum.GetValues(typeof(ItemCategory)))
            {
                if (!_playerData.Inventory.Items.ContainsKey(type)) continue;

                if (!_invetoryGrids.ContainsKey(type))
                    _invetoryGrids.Add(type, new(
                        _inventoryItemSlotFactory,
                        _playerData,
                        gridConfig,
                        itemSlotsContainer[type]
                    ));

                _invetoryGrids[type].GenerateItemSlots(_playerData.Inventory.Items[type]);
            }
        }

    }
}
