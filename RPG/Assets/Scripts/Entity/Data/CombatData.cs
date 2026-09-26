using UnityEngine;

namespace Entity
{
    [System.Serializable]
    public class CombatData
    {
        [field: SerializeField] public LayerMask TargetLayerMask { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
    }
}
