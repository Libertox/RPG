using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Weapon Config", menuName = "ScriptableObjects/Inventory System/Weapon Config")]
    public class WeaponConfig : ItemConfigBase, IComparableItem
    {
        public float ComparisonValue => MainStatisticModifier.ModifierValue;

    }
}
