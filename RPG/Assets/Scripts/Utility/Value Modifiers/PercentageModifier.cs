using UnityEngine;

namespace Utility
{
    public class PercentageModifier : IValueModifier
    {
        public float Apply(float baseValue, float modifier)
        {
            return baseValue * (1f + modifier);
        }

        public float Revert(float modifiedValue, float modifier)
        {
            return modifiedValue / (1f + modifier);
        }
    }
}
