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
        [SerializeField] private float topPadding;
        [SerializeField] private float leftPadding;

        [SerializeField] private float itemPadding;

        [SerializeField] private float itemSlotWidth;
        [SerializeField] private float itemSlotHeight;

        [SerializeField] private float columnNumber;

        private List<InventoryItemSlot> _inventoryItemSlots;

        private PlayerData _playerData;
        private InputManager _inputManager;
        private UIViewManager _viewManager;

        private ItemCategory _selectedItemCategory;

        private List<Vector2Int> _inventoryGrid = new();

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

            SetSelectedItemCategory(ItemCategory.Material);

            foreach(var categoryButton in itemCategoryButtons)
            {
                categoryButton.OnItemCategorySelected += SetSelectedItemCategory;
            }
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

                GenerateItemSlots(_playerData.Inventory.Items[type], itemSlotsContainer[type]);
            }
        }

        private void GenerateItemSlots(List<ItemInventory> items, RectTransform slotContainer)
        {
            _inventoryGrid.Clear();

            int currentRow = 0;
            int currentColumn = 0;

            slotContainer.sizeDelta = new Vector2(slotContainer.sizeDelta.x, 15);

            for (int i = 0;  i < items.Count; i++)
            {
                if(_inventoryGrid.Contains(new(currentColumn, currentRow)))
                {
                    i--;

                    currentColumn++;

                    if (currentColumn == columnNumber)
                    {
                        currentColumn = 0;
                        currentRow++;
                        slotContainer.sizeDelta += new Vector2(0, itemSlotHeight);
                    }

                    continue;
                }

                InventoryItemSlot inventoryItemSlot = Instantiate(inventoryItemSlotPrefab, slotContainer);

                var itemSettings = items[i].ItemBase;

                RectTransform slotRectTransform = (RectTransform)inventoryItemSlot.transform;

                slotRectTransform.anchoredPosition = new Vector2(currentColumn * itemSlotWidth + leftPadding, -(currentRow * itemSlotHeight + topPadding));

                if(currentColumn != 0) slotRectTransform.anchoredPosition += new Vector2(itemPadding, 0f) * currentColumn;
          
                if(currentRow != 0) slotRectTransform.anchoredPosition -= new Vector2(0, itemPadding) * currentRow;

                slotRectTransform.sizeDelta = new Vector2(itemSettings.InventorySize.x * itemSlotWidth, itemSettings.InventorySize.y * itemSlotHeight);

                for (int j = 0; j < itemSettings.InventorySize.x - 1; j++)
                {
                    slotRectTransform.sizeDelta += new Vector2(itemPadding, 0);
                }

                for (int j = 0; j < itemSettings.InventorySize.y - 1; j++)
                {
                    slotRectTransform.sizeDelta += new Vector2(0, itemPadding);
                }

                for (int j = 0; j < itemSettings.InventorySize.x; j++)
                {
                    for(int k = 0; k < itemSettings.InventorySize.y; k++)
                    {
                        _inventoryGrid.Add(new Vector2Int(currentColumn + j, currentRow + k));
                    }
                }

                currentColumn++;
       
                if (currentColumn == columnNumber)
                {
                    currentColumn = 0;
                    currentRow++;
                    slotContainer.sizeDelta += new Vector2(0, itemSlotHeight);
                }
                  

            }
        }
    }
}
