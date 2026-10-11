namespace Utility
{
    public interface IValueModifier
    {
        public float Apply(float baseValue, float modifier);
        public float Revert(float modifiedValue, float modifier);
    }
}
