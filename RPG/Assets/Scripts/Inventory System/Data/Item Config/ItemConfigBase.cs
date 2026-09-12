using UnityEngine;
using Utility.Attribute;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Item Config Base", menuName = "ScriptableObjects/Inventory System/Item Config")]
    public class ItemConfigBase : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public ItemCategory Category { get; private set; }
        [field: SerializeField] public RarityCategory Rarity { get; private set; }


        [field: Header("Statistics")]
        [field: SerializeField] public float Weight { get; private set; }
        [field: SerializeField] public float Gold { get; private set; }
        [field: SerializeField] public float RequiredLevel {  get; private set; }


        [field: Header("Inventory Configures")]
        [field: SerializeField] public Vector2Int InventorySize { get; private set; } = Vector2Int.one;
        [field: SerializeField] public bool CanEquip { get; private set; }
        [field: SerializeField, ShowIf(nameof(CanEquip))] public EquipmentSlotCategory EquipmentSlot { get; private set; }
        [field: SerializeField] public bool CanStack { get; private set; } = true;
        [field: SerializeField, ShowIf(nameof(CanStack))] public int MaxStackSize { get; private set; } = 1;
    }
}
