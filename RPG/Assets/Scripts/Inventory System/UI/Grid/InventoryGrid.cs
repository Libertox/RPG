using Entity.Player;
using InputSystem;
using System.Collections.Generic;
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

        private readonly Dictionary<ItemConfigBase, List<InventoryGridNode>> _nodes = new();
        private InventorySlotFactory _inventorySlotFactory;

        private PlayerInventory _playerInventory;

        [Inject]
        private void Construct(InventoryItemSlotPool inventoryItemSlotPool, PlayerController playerController)
        {
            _inventorySlotFactory = new(inventoryItemSlotPool);
            _playerInventory = playerController.PlayerInventory;
        }

        public bool Drop(InventoryItem item) 
        {
            if(item == null) return false;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(SlotsContainer, InputManager.GetMousePosition(), null, out Vector2 localPoint);

            int column = Mathf.FloorToInt((localPoint.x - config.LeftPadding) / (config.ItemSlotSize.x + config.ItemPadding));
            int row = Mathf.FloorToInt((-localPoint.y - config.TopPadding) / (config.ItemSlotSize.y + config.ItemPadding));

            if (AddItemToGrid(item, column, row))
            {
                _playerInventory.InventoryStorage.AddItem(item.ItemBase);
                return true;
            }

            return false;
        }

        public InventoryItem Get()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(SlotsContainer, InputManager.GetMousePosition(), null, out Vector2 localPoint);

            int column = Mathf.FloorToInt((localPoint.x - config.LeftPadding) / (config.ItemSlotSize.x + config.ItemPadding));
            int row = Mathf.FloorToInt((-localPoint.y - config.TopPadding) / (config.ItemSlotSize.y + config.ItemPadding));

            var node = GetNodeAtPosition(new Vector2Int(column, row));

            if (node == null) return null;

            return node.Slot.Item;
        }


        public void GenerateItemSlots(List<InventoryItem> items)
        {
            if (items == null) return;

            for (int i = 0; i < items.Count; i++)
            {
                AddItemToGrid(items[i], 0, 0);
            }
        }

        private void AddNode(Vector2Int gridPosition, Vector2 worldPosition, InventoryItemSlot slot)
        {
            var node = new InventoryGridNode(gridPosition, worldPosition, slot);

            if (!_nodes.ContainsKey(slot.Item.ItemBase))
                _nodes.Add(slot.Item.ItemBase, new());

            _nodes[slot.Item.ItemBase].Add(node);
        }

        private void RemoveNode(ItemConfigBase itemConfig)
        {
            _nodes.Remove(itemConfig);
        }

        private bool ContainNodeAtPosition(Vector2Int gridPosition)
        {
            foreach(var node in _nodes)
            {
                for (int i = 0; i < node.Value.Count; i++)
                {
                    if (node.Value[i].GridPosition == gridPosition && node.Value[i].Slot.Item != null)
                        return true;
                }
            }

            return false;
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

        public void SetItemOnItemSlot(InventoryItem currentItem, InventoryItem newItem)
        {
            Debug.Log("SetItemOnItemSlot: " + currentItem.ItemBase.Name + " -> " + newItem.ItemBase.Name);

            var node = FindNodeByItem(currentItem.ItemBase);

            if (node == null)
            {
                AddItemToGrid(newItem);
                return;
            }

            var oldBase = currentItem.ItemBase;
            var newBase = newItem.ItemBase;

            if (_nodes.TryGetValue(oldBase, out var oldNodes))
            {
                var nodesToMove = oldNodes.FindAll(n => n.Slot == node.Slot);

                foreach (var n in nodesToMove)
                    oldNodes.Remove(n);

                if (oldNodes.Count == 0)
                    _nodes.Remove(oldBase);

                node.Slot.Initialize(newItem);

                if (!_nodes.ContainsKey(newBase))
                    _nodes.Add(newBase, new List<InventoryGridNode>());

                _nodes[newBase].AddRange(nodesToMove);
            }
            else
            {
                node.Slot.Initialize(newItem);
            }
        }

        public void RemoveItemFromGrid(ItemConfigBase itemBase)
        {
            Debug.Log("RemoveItemFromGrid: " + itemBase.Name);

            var node = FindNodeByItem(itemBase);
            if (node == null) return;

            Unbind(node.Slot);

            _inventorySlotFactory.Release(node.Slot);

            RemoveNode(itemBase);  
        }

        public bool AddItemToGrid(InventoryItem item, int column = 0, int row = 0)
        {
            if(item == null) return false;

            var itemSettings = item.ItemBase;

            if (FindNodeByItem(itemSettings) != null) return false;

            SlotsContainer.sizeDelta = new Vector2(SlotsContainer.sizeDelta.x, CONTAINER_HEIGHT);

            _currentColumn = column;
            _currentRow = row;

            while (ContainNodeAtPosition(new(_currentColumn, _currentRow)))
            {
                IncreaseColumnCount();
            }

            Vector2 slotPosition = new(_currentColumn * config.ItemSlotSize.x + config.LeftPadding, -(_currentRow * config.ItemSlotSize.y + config.TopPadding));
            Vector2 slotSize = new(itemSettings.InventorySize.x * config.ItemSlotSize.x, itemSettings.InventorySize.y * config.ItemSlotSize.y);

            InventoryItemSlot inventoryItemSlot = _inventorySlotFactory.Create(item, SlotsContainer, slotPosition, slotSize);

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

        private void Bind(InventoryItemSlot slot)
        {
            slot.OnSelected += OnItemSlotSelected;
            slot.OnDeselected += OnItemSlotDeselected;
        }

        private void Unbind(InventoryItemSlot slot)
        {
            slot.OnSelected -= OnItemSlotSelected;
            slot.OnDeselected -= OnItemSlotDeselected;
        }

        private void OnItemSlotDeselected()
        {
            itemDescription.Hide();
        }

        private void OnItemSlotSelected(InventoryItemSlot slot)
        {
            itemDescription.Setup(slot.Item);

            itemDescription.ShowAtPosition(slot.GetRightBottomCornerPosition());
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
            if (!_nodes.ContainsKey(item))
                return null;

            _nodes.TryGetValue(item, out List<InventoryGridNode> nodes);

            return nodes[0];
        }
    }
}
