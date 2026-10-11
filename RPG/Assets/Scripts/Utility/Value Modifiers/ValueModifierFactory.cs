namespace Utility
{
    public class ValueModifierFactory
    {
        public static IValueModifier Create(ModifierType modifierType)
        {
            switch (modifierType)
            {
                case ModifierType.Addition: return new AdditionModifier();
                case ModifierType.Percentage: return new PercentageModifier();
                default: break;
            }

            return default;
        }
    }
}
