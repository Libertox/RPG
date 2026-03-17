

using System;
using UnityEngine;

namespace UI.Inventory
{
    [Serializable]
    public class InventoryGridConfig
    {
        [field: SerializeField] public float TopPadding { get; private set; }
        [field: SerializeField] public float LeftPadding { get; private set; }
        [field: SerializeField] public float ItemPadding { get; private set; }

        [field: SerializeField] public Vector2 ItemSlotSize { get; private set; }
        [field: SerializeField] public float ColumnNumber { get; private set; }

    }
}
