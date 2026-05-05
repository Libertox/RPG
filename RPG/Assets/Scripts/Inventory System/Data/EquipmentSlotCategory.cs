using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Equipment Slot Category", menuName = "ScriptableObjects/Inventory System/Equipment Slot Category")]
    public class EquipmentSlotCategory : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }


    }
}
