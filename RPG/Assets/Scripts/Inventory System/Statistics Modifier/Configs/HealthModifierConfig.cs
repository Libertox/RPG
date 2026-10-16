using UnityEngine;

namespace InventorySystem.StatisticModifiers
{
    [CreateAssetMenu(fileName = "Health Modifier", menuName = "ScriptableObjects/Statistic Modifier/Health Modifier")]
    public class HealthModifierConfig : StatisticModifierConfig
    {
        public override StatisticModifier Create()
        {
            return new HealthModifier(this);
        }    
    }
}
