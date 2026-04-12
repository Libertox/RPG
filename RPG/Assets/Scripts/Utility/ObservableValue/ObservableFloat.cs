

using UnityEngine;

namespace Utility
{
    public class ObservableFloat : ObservableValue<float>
    {
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
