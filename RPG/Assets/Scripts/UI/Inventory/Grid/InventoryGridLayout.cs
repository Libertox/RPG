using System.Collections.Generic;
using UnityEngine;

namespace UI.Inventory
{
    public class InventoryGridLayout
    {
        private readonly InventoryGridConfig _config;

        private readonly Dictionary<Vector2Int, InventoryGridNode> _occupied = new();

        public InventoryGridLayout(InventoryGridConfig config)
        {
            _config = config;
        }

        public Vector2Int FindFreePosition(Vector2Int size)
        {
            int row = 0;

            while (true)
            {
                for (int col = 0; col < _config.ColumnNumber; col++)
                {
                    var pos = new Vector2Int(col, row);

                    if (CanPlace(pos, size))
                        return pos;
                }

                row++;
            }
        }

        private bool CanPlace(Vector2Int start, Vector2Int size)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    var pos = new Vector2Int(start.x + x, start.y + y);

                    if (_occupied.ContainsKey(pos))
                        return false;
                }
            }

            return true;
        }

        public List<InventoryGridNode> CreateNodes(Vector2Int start, Vector2Int size, InventoryItemSlot slot)
        {
            var nodes = new List<InventoryGridNode>();

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    var pos = new Vector2Int(start.x + x, start.y + y);

                    var node = new InventoryGridNode(pos, slot.RectTransform.anchoredPosition, slot);

                    _occupied[pos] = node;
                    nodes.Add(node);
                }
            }

            return nodes;
        }

        public void RemoveNodes(List<InventoryGridNode> nodes)
        {
            foreach (var node in nodes)
            {
                _occupied.Remove(node.GridPosition);
            }
        }

        public Vector2 GetWorldPosition(Vector2Int gridPosition)
        {
            return new Vector2(
                gridPosition.x * _config.ItemSlotSize.x + _config.LeftPadding,
               -(gridPosition.y * _config.ItemSlotSize.y + _config.TopPadding)
            );
        }

        public Vector2 GetSize(Vector2Int size)
        {
            return new Vector2(
                size.x * _config.ItemSlotSize.x + (size.x - 1) * _config.ItemPadding,
                size.y * _config.ItemSlotSize.y + (size.y - 1) * _config.ItemPadding
            );
        }

        public void Clear()
        {
            _occupied.Clear();
        }

    }
}
