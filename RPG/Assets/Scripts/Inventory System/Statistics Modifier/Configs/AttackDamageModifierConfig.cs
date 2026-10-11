using UnityEngine;

namespace InventorySystem.StatisticModifier
{
    [CreateAssetMenu(fileName = "Attack Damage Modifier", menuName = "ScriptableObjects/Statistic Modifier/Attack Damage Modifier")]
    public class AttackDamageModifierConfig : StatisticModifierConfig
    {
        public override StatisticModifier Create()
        {
            return new AttackDamageModifier(ModifierType, ModifierValue);
        }
    }
}
