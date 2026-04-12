using InventorySystem;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UI.Inventory
{
    public class InventoryGrid : MonoBehaviour
    {
        [SerializeField] private InventoryGridConfig config;
        [SerializeField] private ItemDescriptionView itemDescription;
        [field: SerializeField] public RectTransform SlotsContainer { get; private set; }

        private int _currentRow = 0;
        private int _currentColumn = 0;

        private List<InventoryGridNode> _nodes;
        private InventoryItemSlotPool _inventoryItemSlotFactory;

        [Inject]
        private void Construct(InventoryItemSlotPool inventoryItemSlotFactory)
        {
            _inventoryItemSlotFactory = inventoryItemSlotFactory;
        }

        private void Awake()
        {
            _nodes = new();
        }

        public void GenerateItemSlots(List<ItemInventory> items)
        {
            if (items == null) return;

            SlotsContainer.sizeDelta = new Vector2(SlotsContainer.sizeDelta.x, 15);

            for (int i = 0; i < items.Count; i++)
            {
                var itemSettings = items[i].ItemBase;

                if (FindNodeByItem(itemSettings) != null) continue;

                AddItemToGrid(itemSettings, 0, 0);
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

        public void SetItemOnItemSlot(ItemConfigBase currentItem, ItemConfigBase newItem)
        {
            var node = FindNodeByItem(currentItem);

            if (node == null) return;
    
            node.Slot.Initialize(newItem);
        }

        public void RemoveItemFromGrid(ItemConfigBase itemBase)
        {
            var nodes = FindNodesByItem(itemBase);
            if (nodes == null || nodes.Count == 0) return;

            nodes[0].Slot.OnSelected -= OnItemSlotSelected;
            nodes[0].Slot.OnDeselected -= OnItemSlotDeselected;

            _inventoryItemSlotFactory.Despawn(nodes[0].Slot);

            foreach (var node in nodes)
            {
                RemoveNode(node);
            }   
        }

        public void AddItemToGrid(ItemConfigBase itemSettings, int column = 0, int row = 0)
        {
            _currentColumn = column;
            _currentRow = row;

            while (ContainNodeAtPosition(new(_currentColumn, _currentRow)))
            {
                IncreaseColumnCount();
            }

            Vector2 slotPosition = new(_currentColumn * config.ItemSlotSize.x + config.LeftPadding, -(_currentRow * config.ItemSlotSize.y + config.TopPadding));
            Vector2 slotSize = new(itemSettings.InventorySize.x * config.ItemSlotSize.x, itemSettings.InventorySize.y * config.ItemSlotSize.y);

            InventoryItemSlot inventoryItemSlot = _inventoryItemSlotFactory
                .Spawn()
                .Initialize(itemSettings)
                .SetParent(SlotsContainer)
                .SetAnchoredPosition(slotPosition)
                .SetSize(slotSize);

            inventoryItemSlot.OnSelected += OnItemSlotSelected;
            inventoryItemSlot.OnDeselected += OnItemSlotDeselected;

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
        }

        private void OnItemSlotDeselected()
        {
            itemDescription.Hide();
        }

        private void OnItemSlotSelected(ItemConfigBase item)
        {
            itemDescription.Setup(item);
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

        private InventoryGridNode FindNodeByItem(ItemConfigBase item)
        {
            return _nodes.Find((node) => node.Slot.Item == item);
        }

        private List<InventoryGridNode> FindNodesByItem(ItemConfigBase item)
        {
            return _nodes.FindAll((node) => node.Slot.Item == item);
        }

    }
}
