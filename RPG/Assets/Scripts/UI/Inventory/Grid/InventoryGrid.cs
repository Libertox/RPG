using Entity.Player;
using InputSystem;
using InventorySystem;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI.Inventory
{
    public class InventoryGrid : MonoBehaviour, IDragable
    {
        [SerializeField] private InventoryGridConfig config;
        [SerializeField] private ItemDescriptionView itemDescription;

        [SerializeField] private ItemHolder itemHolder;

        [field: SerializeField] public RectTransform SlotsContainer { get; private set; }

        private int _currentRow = 0;
        private int _currentColumn = 0;

        private readonly Dictionary<ItemConfigBase, List<InventoryGridNode>> _nodes = new();
        private InventorySlotFactory _inventorySlotFactory;

        private InputManager _inputManager;
        private PlayerInventory _playerInventory;

        [Inject]
        private void Construct(InventoryItemSlotPool inventoryItemSlotPool, InputManager inputManager, PlayerController playerController)
        {
            _inventorySlotFactory = new(inventoryItemSlotPool);
            _inputManager = inputManager;
            _playerInventory = playerController.PlayerInventory;
        }

        public void Drag() { }
  
        public void Drop() 
        {
            if(itemHolder.HoldItem == null) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(SlotsContainer, _inputManager.GetMousePosition(), null, out Vector2 localPoint);

            int column = Mathf.FloorToInt((localPoint.x - config.LeftPadding) / (config.ItemSlotSize.x + config.ItemPadding));
            int row = Mathf.FloorToInt((-localPoint.y - config.TopPadding) / (config.ItemSlotSize.y + config.ItemPadding));

            AddItemToGrid(itemHolder.HoldItem, column, row);

            _playerInventory.InventoryStorage.AddItem(itemHolder.HoldItem.ItemBase);
        }
 
        public void GenerateItemSlots(List<ItemInventory> items)
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

        public void SetItemOnItemSlot(ItemInventory currentItem, ItemInventory newItem)
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

                node.Slot.Initialize(newItem, itemHolder);

                if (!_nodes.ContainsKey(newBase))
                    _nodes.Add(newBase, new List<InventoryGridNode>());

                _nodes[newBase].AddRange(nodesToMove);
            }
            else
            {
                node.Slot.Initialize(newItem, itemHolder);
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

        public void AddItemToGrid(ItemInventory item, int column = 0, int row = 0)
        {
            if(item == null) return;

            var itemSettings = item.ItemBase;

            if (FindNodeByItem(itemSettings) != null) return;

            SlotsContainer.sizeDelta = new Vector2(SlotsContainer.sizeDelta.x, 650);

            _currentColumn = column;
            _currentRow = row;

            while (ContainNodeAtPosition(new(_currentColumn, _currentRow)))
            {
                IncreaseColumnCount();
            }

            Vector2 slotPosition = new(_currentColumn * config.ItemSlotSize.x + config.LeftPadding, -(_currentRow * config.ItemSlotSize.y + config.TopPadding));
            Vector2 slotSize = new(itemSettings.InventorySize.x * config.ItemSlotSize.x, itemSettings.InventorySize.y * config.ItemSlotSize.y);

            InventoryItemSlot inventoryItemSlot = _inventorySlotFactory.Create(item, SlotsContainer, slotPosition, slotSize, itemHolder);

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

        private void OnItemSlotSelected(ItemInventory item)
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
            if (!_nodes.ContainsKey(item))
                return null;

            _nodes.TryGetValue(item, out List<InventoryGridNode> nodes);

            return nodes[0];
        }
    }
}
