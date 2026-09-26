

using UnityEngine;

namespace Utility
{
    public class ObservableFloat : ObservableValue<float>
    {
        public ObservableFloat(float value) : base(value)
        {

        }

        public override void Add(float value)
        {
            Value += value;
        }

        public override void Subtract(float value)
        {
            Value = Mathf.Max(0, Value - value);
        }
    }
}
