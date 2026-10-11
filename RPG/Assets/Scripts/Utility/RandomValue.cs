using UnityEngine;

namespace Utility
{
    [System.Serializable]
    public class RandomValue
    {
        [field: SerializeField] public float MinValue { get; private set; }
        [field: SerializeField] public float MaxValue { get; private set; }

        public float GetRandomValue()
        {
            return UnityEngine.Random.Range(MinValue, MaxValue);
        }

    }
}
