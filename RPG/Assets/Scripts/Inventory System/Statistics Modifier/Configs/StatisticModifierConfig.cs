using UnityEngine;
using Utility;
using Utility.Attribute;

namespace InventorySystem.StatisticModifiers
{
    public abstract class StatisticModifierConfig : ScriptableObject
    {
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public ModifierType ModifierType { get; private set; }

        [field: SerializeField] public bool IsRangeModifier { get; private set; }

        [field: SerializeField, HideIf(nameof(IsRangeModifier))] public float ModifierValue { get; private set; }
        [field: SerializeField, ShowIf(nameof(IsRangeModifier))] public RandomValue ModifierValueRange {  get; private set; }

        public abstract StatisticModifier Create();

        public string GetDescription()
        {
            string sign = ModifierType == ModifierType.Addition ? "" : "%";
            float value = ModifierType == ModifierType.Addition ? ModifierValue : ModifierValue * 100;

            return $"+{value}{sign} {Description}";
        }
    }
}
