

using System;
using UnityEngine;

namespace UI.Inventory
{
    [CreateAssetMenu(fileName = "Inventory Grid Config", menuName = "ScriptableObjects/Inventory System/Inventory Grid Config")]
    public class InventoryGridConfig : ScriptableObject
    {
        [field: SerializeField] public float TopPadding { get; private set; }
        [field: SerializeField] public float LeftPadding { get; private set; }
        [field: SerializeField] public float ItemPadding { get; private set; }

        [field: SerializeField] public Vector2 ItemSlotSize { get; private set; }
        [field: SerializeField] public float ColumnNumber { get; private set; }

    }
}
