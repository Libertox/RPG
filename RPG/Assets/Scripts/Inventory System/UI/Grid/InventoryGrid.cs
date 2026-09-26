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
        [SerializeField] private ItemCategory itemCategory;

        [field: SerializeField] public RectTransform SlotsContainer { get; private set; }

        private int currentRow = 0;
        private int currentColumn = 0;

        private readonly Dictionary<InventorySlot, List<InventoryGridNode>> nodes = new();
        private InventorySlotFactory inventorySlotFactory;

        private PlayerInventory playerInventory;
        private PlayerController playerController;

        [Inject]
        private void Construct(InventoryItemSlotPool inventoryItemSlotPool, PlayerInventory playerInventory, PlayerController playerController)
        {
            inventorySlotFactory = new(inventoryItemSlotPool);
            this.playerInventory = playerInventory;
            this.playerController = playerController;
        }

        public bool Drop(InventorySlot item) 
        {
            if(item == null) return false;

            if (item.ItemBase.Category != itemCategory) return false;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(SlotsContainer, InputManager.GetMousePosition(), null, out Vector2 localPoint);

            int column = Mathf.FloorToInt((localPoint.x - config.LeftPadding) / (config.ItemSlotSize.x + config.ItemPadding));
            int row = Mathf.FloorToInt((-localPoint.y - config.TopPadding) / (config.ItemSlotSize.y + config.ItemPadding));

            if (!nodes.ContainsKey(item))
            {
                playerInventory.InventoryStorage.AddItem(item);
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

            if (node.Slot.Item.ItemBase.RequiredLevel > playerController.Statistic.Level.Value)
                return null;

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

        public void Sort()
        {
            List<InventorySlot> inventorySlots = nodes.Keys.ToList();

            foreach (InventorySlot slot in inventorySlots)
            {
                RemoveItemFromGrid(slot);
            }

            nodes.Clear();
            Refresh(inventorySlots);
        }

        private void RemoveUnusedNodes(List<InventorySlot> items)
        {
            foreach (var key in nodes.Keys.ToList())
            {
                if (!items.Contains(key))
                {
                    Debug.Log("Remove From Unused Nodes");
                    RemoveItemFromGrid(key);
                }              
            }
        }

        private void AddNode(Vector2Int gridPosition, Vector2 worldPosition, InventorySlotUI slot)
        {
            var node = new InventoryGridNode(gridPosition, worldPosition, slot);

            if (!nodes.ContainsKey(slot.Item))
                nodes.Add(slot.Item, new());

            nodes[slot.Item].Add(node);
        }

        private void RemoveNode(InventorySlot item)
        {
            nodes.Remove(item);
        }

        private InventoryGridNode GetNodeAtPosition(Vector2Int gridPosition)
        {
            foreach (var node in nodes)
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
            Debug.Log("RemoveItemFromGrid: " + item.ItemBase.Name);

            var node = FindNodeByItem(item);
            if (node == null) return;

            inventorySlotFactory.Release(node.Slot);

            RemoveNode(item);  
        }

        public bool AddItemToGrid(InventorySlot item, int column = 0, int row = 0)
        {
            if (item == null) return false;

            if (nodes.ContainsKey(item))
            {
                var node = FindNodeByItem(item);

                node?.Slot.Initialize(item);

                return false;
            }

            var itemSettings = item.ItemBase;

            SlotsContainer.sizeDelta = new Vector2(SlotsContainer.sizeDelta.x, CONTAINER_HEIGHT);

            currentColumn = column;
            currentRow = row;

            while (!CanPlaceItem(new Vector2Int(currentColumn, currentRow), item.ItemBase.InventorySize))
            {
                IncreaseColumnCount();
            }

            Vector2 slotPosition = new(currentColumn * config.ItemSlotSize.x + config.LeftPadding, -(currentRow * config.ItemSlotSize.y + config.TopPadding));
            Vector2 slotSize = new(itemSettings.InventorySize.x * config.ItemSlotSize.x, itemSettings.InventorySize.y * config.ItemSlotSize.y);

            InventorySlotUI inventoryItemSlot = inventorySlotFactory.Create(item, SlotsContainer, slotPosition, slotSize);

            if (currentColumn != 0)
                inventoryItemSlot.RectTransform.anchoredPosition += new Vector2(config.ItemPadding, 0f) * currentColumn;

            if (currentRow != 0)
                inventoryItemSlot.RectTransform.anchoredPosition -= new Vector2(0, config.ItemPadding) * currentRow;

            for (int j = 0; j < itemSettings.InventorySize.x - 1; j++)
                inventoryItemSlot.RectTransform.sizeDelta += new Vector2(config.ItemPadding, 0);

            for (int j = 0; j < itemSettings.InventorySize.y - 1; j++)
                inventoryItemSlot.RectTransform.sizeDelta += new Vector2(0, config.ItemPadding);


            for (int j = 0; j < itemSettings.InventorySize.x; j++)
            {
                for (int k = 0; k < itemSettings.InventorySize.y; k++)
                {
                    var gridPosition = new Vector2Int(currentColumn + j, currentRow + k);

                    AddNode(gridPosition, inventoryItemSlot.RectTransform.anchoredPosition, inventoryItemSlot);
                }
            }

            IncreaseColumnCount();

            Debug.Log("AddItemToGrid: " + item.ItemBase.Name);

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

        private void IncreaseColumnCount()
        {
            currentColumn++;

            if (currentColumn == config.ColumnNumber)
            {
                currentColumn = 0;
                currentRow++;
                SlotsContainer.sizeDelta += new Vector2(0, config.ItemSlotSize.y);
            }
        }

        private InventoryGridNode FindNodeByItem(InventorySlot item)
        {
            if (!nodes.TryGetValue(item, out var findedNodes))
                return null;

            return findedNodes.Count > 0 ? findedNodes[0] : null;
        }
    }
}
