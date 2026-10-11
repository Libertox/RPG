using UnityEngine;

namespace InventorySystem.StatisticModifier
{
    [CreateAssetMenu(fileName = "Movement Speed Modifier", menuName = "ScriptableObjects/Statistic Modifier/Movement Speed Modifier")]
    public class MovementSpeedModifierConfig : StatisticModifierConfig
    {
        public override IStatisticModifier Create()
        {
            return new MovementSpeedModifier(ModifierType, ModifierValue);
        }
    }


}
