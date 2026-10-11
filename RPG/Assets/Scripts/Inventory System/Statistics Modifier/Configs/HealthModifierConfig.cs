using UnityEngine;

namespace InventorySystem.StatisticModifier
{
    [CreateAssetMenu(fileName = "Health Modifier", menuName = "ScriptableObjects/Statistic Modifier/Health Modifier")]
    public class HealthModifierConfig : StatisticModifierConfig
    {
        public override IStatisticModifier Create()
        {
            return new HealthModifier(ModifierType, ModifierValue);
        }    
    }
}
