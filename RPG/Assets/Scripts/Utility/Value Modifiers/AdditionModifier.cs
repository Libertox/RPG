namespace Utility
{
    public class AdditionModifier : IValueModifier
    {
        public float Apply(float baseValue, float modifier)
        {
            return baseValue + modifier;
        }

        public float Revert(float modifiedValue, float modifier)
        {
            return modifiedValue - modifier;
        }
    }
}
