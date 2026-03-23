using Entity.Player;
using InventorySystem;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Inventory
{
    public class InventoryGrid
    {
        private readonly List<InventoryGridNode> _nodes;

        private readonly InventoryGridConfig _config;

        private int _currentRow = 0;
        private int _currentColumn = 0;

        private readonly InventoryItemSlotFactory _inventoryItemSlotFactory;
        private readonly PlayerData _playerData;
        private readonly RectTransform _slotsContainer;

        public InventoryGrid(InventoryItemSlotFactory inventoryItemSlotFactory, PlayerData playerData, 
            InventoryGridConfig config, RectTransform slotsContainer)
        {
            _nodes = new();

            _inventoryItemSlotFactory = inventoryItemSlotFactory;
            _playerData = playerData;
            _config = config;
            _slotsContainer = slotsContainer;
        }

        public void GenerateItemSlots(List<ItemInventory> items)
        {
            _slotsContainer.sizeDelta = new Vector2(_slotsContainer.sizeDelta.x, 15);

            for (int i = 0; i < items.Count; i++)
            {
                var itemSettings = items[i].ItemBase;

                if (FindNodeByItem(itemSettings) != null) continue;

                AddItemToGrid(itemSettings, _currentColumn, _currentRow);
            }
        }

        private void AddNode(Vector2Int gridPosition, Vector2 worldPosition, InventoryItemSlot slot)
        {
            var node = new InventoryGridNode(gridPosition, worldPosition, slot);

            _nodes.Add(node);
        }

        private void RemoveNode(InventoryGridNode gridNode)
        {
            _nodes.Remove(gridNode);
        }

        private bool ContainNodeAtPosition(Vector2Int gridPosition)
        {
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i].GridPosition == gridPosition && _nodes[i].Slot.Item != null)
                    return true;
            }

            return false;
        }

        public void SetItemOnItemSlot(ItemBase newItem, ItemBase targetPosition)
        {
            var node = FindNodeByItem(targetPosition);

            node.Slot.Initialize(newItem, _playerData.Inventory);
        }

        public void RemoveItemFromGrid(ItemBase itemBase)
        {
            var nodes = FindNodesByItem(itemBase);
            if (nodes == null) return;

            _inventoryItemSlotFactory.Destory(nodes[0].Slot);

            foreach (var node in nodes)
            {
                RemoveNode(node);
            }   
        }

        public void AddItemToGrid(ItemBase itemSettings, int column = 0, int row = 0)
        {
            _currentColumn = column;
            _currentRow = row;

            while (ContainNodeAtPosition(new(_currentColumn, _currentRow)))
            {
                IncreaseColumnCount();
            }

            Vector2 slotPosition = new(_currentColumn * _config.ItemSlotSize.x + _config.LeftPadding, -(_currentRow * _config.ItemSlotSize.y + _config.TopPadding));
            Vector2 slotSize = new(itemSettings.InventorySize.x * _config.ItemSlotSize.x, itemSettings.InventorySize.y * _config.ItemSlotSize.y);

            InventoryItemSlot inventoryItemSlot = _inventoryItemSlotFactory
                .Create()
                .Initialize(itemSettings, _playerData.Inventory)
                .SetParent(_slotsContainer)
                .SetAnchoredPosition(slotPosition)
                .SetSize(slotSize);

            if (_currentColumn != 0)
                inventoryItemSlot.RectTransform.anchoredPosition += new Vector2(_config.ItemPadding, 0f) * _currentColumn;

            if (_currentRow != 0)
                inventoryItemSlot.RectTransform.anchoredPosition -= new Vector2(0, _config.ItemPadding) * _currentRow;

            for (int j = 0; j < itemSettings.InventorySize.x - 1; j++)
                inventoryItemSlot.RectTransform.sizeDelta += new Vector2(_config.ItemPadding, 0);

            for (int j = 0; j < itemSettings.InventorySize.y - 1; j++)
                inventoryItemSlot.RectTransform.sizeDelta += new Vector2(0, _config.ItemPadding);


            for (int j = 0; j < itemSettings.InventorySize.x; j++)
            {
                for (int k = 0; k < itemSettings.InventorySize.y; k++)
                {
                    var gridPosition = new Vector2Int(_currentColumn + j, _currentRow + k);

                    AddNode(gridPosition, inventoryItemSlot.RectTransform.anchoredPosition, inventoryItemSlot);
                }
            }

            IncreaseColumnCount();
        }

        private void IncreaseColumnCount()
        {
            _currentColumn++;

            if (_currentColumn == _config.ColumnNumber)
            {
                _currentColumn = 0;
                _currentRow++;
                _slotsContainer.sizeDelta += new Vector2(0, _config.ItemSlotSize.y);
            }
        }

        private InventoryGridNode FindNodeByItem(ItemBase item)
        {
            return _nodes.Find((node) => node.Slot.Item == item);
        }

        private List<InventoryGridNode> FindNodesByItem(ItemBase item)
        {
            return _nodes.FindAll((node) => node.Slot.Item == item);
        }

    }
}
