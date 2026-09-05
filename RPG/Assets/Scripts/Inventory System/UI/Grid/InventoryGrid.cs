using Entity.Player;
using InputSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace InventorySystem.UI
{
    public class InventoryGrid : MonoBehaviour, IItemContainer
    {
        private const int CONTAINER_HEIGHT = 650;

        [SerializeField] private InventoryGridConfig config;
        [SerializeField] private ItemDescriptionView itemDescription;
        [field: SerializeField] public RectTransform SlotsContainer { get; private set; }

        private int _currentRow = 0;
        private int _currentColumn = 0;

        private readonly Dictionary<InventorySlot, List<InventoryGridNode>> _nodes = new();
        private InventorySlotFactory _inventorySlotFactory;

        private PlayerInventory _playerInventory;

        [Inject]
        private void Construct(InventoryItemSlotPool inventoryItemSlotPool, PlayerController playerController)
        {
            _inventorySlotFactory = new(inventoryItemSlotPool);
            _playerInventory = playerController.PlayerInventory;
        }

        public bool Drop(InventorySlot item) 
        {
            if(item == null) return false;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(SlotsContainer, InputManager.GetMousePosition(), null, out Vector2 localPoint);

            int column = Mathf.FloorToInt((localPoint.x - config.LeftPadding) / (config.ItemSlotSize.x + config.ItemPadding));
            int row = Mathf.FloorToInt((-localPoint.y - config.TopPadding) / (config.ItemSlotSize.y + config.ItemPadding));

            if (!_nodes.ContainsKey(item))
            {
                _playerInventory.InventoryStorage.AddItem(item);
                AddItemToGrid(item, column, row);
                return true;
            }

            return false;
        }

        public InventorySlot Get()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(SlotsContainer, InputManager.GetMousePosition(), null, out Vector2 localPoint);

            int column = Mathf.FloorToInt((localPoint.x - config.LeftPadding) / (config.ItemSlotSize.x + config.ItemPadding));
            int row = Mathf.FloorToInt((-localPoint.y - config.TopPadding) / (config.ItemSlotSize.y + config.ItemPadding));

            var node = GetNodeAtPosition(new Vector2Int(column, row));

            if (node == null) return null;

            return node.Slot.Item;
        }


        public void Refresh(List<InventorySlot> items)
        {
            if (items == null) return;

            for (int i = 0; i < items.Count; i++)
            {
                AddItemToGrid(items[i], 0, 0);
            }

            RemoveUnusedNodes(items);
        }

        private void RemoveUnusedNodes(List<InventorySlot> items)
        {
            foreach (var key in _nodes.Keys.ToList())
            {
                if (!items.Contains(key))
                    RemoveItemFromGrid(key);
            }
        }

        private void AddNode(Vector2Int gridPosition, Vector2 worldPosition, InventorySlotUI slot)
        {
            var node = new InventoryGridNode(gridPosition, worldPosition, slot);

            if (!_nodes.ContainsKey(slot.Item))
                _nodes.Add(slot.Item, new());

            _nodes[slot.Item].Add(node);
        }

        private void RemoveNode(InventorySlot item)
        {
            _nodes.Remove(item);
        }

        private InventoryGridNode GetNodeAtPosition(Vector2Int gridPosition)
        {
            foreach (var node in _nodes)
            {
                for (int i = 0; i < node.Value.Count; i++)
                {
                    if (node.Value[i].GridPosition == gridPosition && node.Value[i].Slot.Item != null)
                        return node.Value[i];
                }
            }

            return null;
        }

        public void ReplaceItem(InventorySlot currentItem, InventorySlot newItem)
        {
            var node = FindNodeByItem(currentItem);

            if (node == null)
            {
                return;
            }
            RemoveItemFromGrid(currentItem);

            var gridPosition = node.GridPosition;

            AddItemToGrid(newItem, gridPosition.x, gridPosition.y);
        }

        public void RemoveItemFromGrid(InventorySlot item)
        {
            Debug.LogError("RemoveItemFromGrid: " + item.ItemBase.Name);

            var node = FindNodeByItem(item);
            if (node == null) return;

            Unbind(node.Slot);

            _inventorySlotFactory.Release(node.Slot);

            RemoveNode(item);  
        }

        public bool AddItemToGrid(InventorySlot item, int column = 0, int row = 0)
        {
            if (item == null) return false;

            if (_nodes.ContainsKey(item))
            {
                var node = FindNodeByItem(item);

                node?.Slot.Initialize(item);

                return false;
            }

            var itemSettings = item.ItemBase;

            SlotsContainer.sizeDelta = new Vector2(SlotsContainer.sizeDelta.x, CONTAINER_HEIGHT);

            _currentColumn = column;
            _currentRow = row;

            while (!CanPlaceItem(new Vector2Int(_currentColumn, _currentRow), item.ItemBase.InventorySize))
            {
                IncreaseColumnCount();
            }

            Vector2 slotPosition = new(_currentColumn * config.ItemSlotSize.x + config.LeftPadding, -(_currentRow * config.ItemSlotSize.y + config.TopPadding));
            Vector2 slotSize = new(itemSettings.InventorySize.x * config.ItemSlotSize.x, itemSettings.InventorySize.y * config.ItemSlotSize.y);

            InventorySlotUI inventoryItemSlot = _inventorySlotFactory.Create(item, SlotsContainer, slotPosition, slotSize);

            Bind(inventoryItemSlot);

            if (_currentColumn != 0)
                inventoryItemSlot.RectTransform.anchoredPosition += new Vector2(config.ItemPadding, 0f) * _currentColumn;

            if (_currentRow != 0)
                inventoryItemSlot.RectTransform.anchoredPosition -= new Vector2(0, config.ItemPadding) * _currentRow;

            for (int j = 0; j < itemSettings.InventorySize.x - 1; j++)
                inventoryItemSlot.RectTransform.sizeDelta += new Vector2(config.ItemPadding, 0);

            for (int j = 0; j < itemSettings.InventorySize.y - 1; j++)
                inventoryItemSlot.RectTransform.sizeDelta += new Vector2(0, config.ItemPadding);


            for (int j = 0; j < itemSettings.InventorySize.x; j++)
            {
                for (int k = 0; k < itemSettings.InventorySize.y; k++)
                {
                    var gridPosition = new Vector2Int(_currentColumn + j, _currentRow + k);

                    AddNode(gridPosition, inventoryItemSlot.RectTransform.anchoredPosition, inventoryItemSlot);
                }
            }

            IncreaseColumnCount();

            return true;
        }

        private bool CanPlaceItem(Vector2Int position, Vector2Int size)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    if (GetNodeAtPosition(position + new Vector2Int(x, y)) != null)
                        return false;
                }
            }

            return true;
        }

        private void Bind(InventorySlotUI slot)
        {
            slot.OnSelected += OnItemSlotSelected;
            slot.OnDeselected += OnItemSlotDeselected;
        }

        private void Unbind(InventorySlotUI slot)
        {
            slot.OnSelected -= OnItemSlotSelected;
            slot.OnDeselected -= OnItemSlotDeselected;
        }

        private void OnItemSlotDeselected()
        {
            //itemDescription.Hide();
        }

        private void OnItemSlotSelected(InventorySlotUI slot)
        {
            //itemDescription.Setup(slot.Item);

            //itemDescription.ShowAtPosition(slot.GetRightBottomCornerPosition());
        }

        private void IncreaseColumnCount()
        {
            _currentColumn++;

            if (_currentColumn == config.ColumnNumber)
            {
                _currentColumn = 0;
                _currentRow++;
                SlotsContainer.sizeDelta += new Vector2(0, config.ItemSlotSize.y);
            }
        }

        private InventoryGridNode FindNodeByItem(InventorySlot item)
        {
            if (!_nodes.TryGetValue(item, out var nodes))
                return null;

            return nodes.Count > 0 ? nodes[0] : null;
        }
    }
}
