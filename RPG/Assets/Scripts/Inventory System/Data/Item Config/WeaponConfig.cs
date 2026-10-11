using Entity.Player;
using InventorySystem.StatisticModifier;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Weapon Config", menuName = "ScriptableObjects/Inventory System/Weapon Config")]
    public class WeaponConfig : ItemConfigBase, IComparableItem, IStatModifierProvider
    {
        [field: SerializeField] public StatisticModifierConfig MainStatisticModifier {  get; private set; }
        [field: SerializeField] public StatisticModifierConfig[] AdditionalStatisticModifiers { get; private set; }

        public float ComparisonValue => MainStatisticModifier.ModifierValue;
    }
}
