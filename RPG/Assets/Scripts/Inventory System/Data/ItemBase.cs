

using UnityEngine;
using Utility.Attribute;

namespace InventorySystem
{

    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Inventory System/Inventory Item")]
    public class ItemBase : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public ItemCategory Type { get; private set; }
        [field: SerializeField] public float Weight { get; private set; }
        [field: SerializeField] public Vector2Int InventorySize { get; private set; } = Vector2Int.one;

        [field: SerializeField] public bool CanEquip { get; private set; }
        [field: SerializeField, ShowIf(nameof(CanEquip))] public EquipmentSlot EquipmentSlot { get; private set; }
    }
}
