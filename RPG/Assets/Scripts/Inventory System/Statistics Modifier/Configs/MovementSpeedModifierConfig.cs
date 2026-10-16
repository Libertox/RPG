using UnityEngine;

namespace InventorySystem.StatisticModifiers
{
    [CreateAssetMenu(fileName = "Movement Speed Modifier", menuName = "ScriptableObjects/Statistic Modifier/Movement Speed Modifier")]
    public class MovementSpeedModifierConfig : StatisticModifierConfig
    {
        public override StatisticModifier Create()
        {
            return new MovementSpeedModifier(this);
        }
    }


}
