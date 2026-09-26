

namespace Utility
{
    public class ObservableInt : ObservableValue<int>
    {
        public ObservableInt(int value) : base(value)
        {

        }

        public override void Add(int value)
        {
            Value += value;
        }

        public override void Subtract(int value)
        {
            Value -= value;
        }
    }
}
