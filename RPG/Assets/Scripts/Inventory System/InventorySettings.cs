using UnityEngine;
using Utility;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Inventory Settings", menuName = "ScriptableObjects/Inventory System/Inventory Settings")]
    public class InventorySettings : ScriptableObject
    {
        [field: SerializeField] public SerializableDictionary<EquipmentSlotCategory, int> MaxEquipmentSlotsAmount { get; private set; }

    }
}
