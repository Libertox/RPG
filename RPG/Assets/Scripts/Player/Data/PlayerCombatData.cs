

using Player.Data;
using UnityEngine;

namespace Player.Data
{
    [CreateAssetMenu(fileName = nameof(PlayerCombatData), menuName = "ScriptableObjects/Player/" + nameof(PlayerCombatData))]
    public class PlayerCombatData : ScriptableObject
    {
        [field: SerializeField] public LayerMask TargetLayerMask { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
    }
}
