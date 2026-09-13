using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Weapon Config", menuName = "ScriptableObjects/Inventory System/Weapon Config")]
    public class WeaponConfig : ItemConfigBase, IComparableItem
    {
        [field: SerializeField] public float Damage { get; private set; }

        public float ComparisonValue => Damage;
    }
}
