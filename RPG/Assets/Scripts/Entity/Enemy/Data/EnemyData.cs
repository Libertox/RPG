using UnityEngine;

namespace Entity.Enemy
{
    [CreateAssetMenu(fileName = nameof(EnemyData), menuName = "ScriptableObjects/Enemy/"+nameof(EnemyData))]
    public class EnemyData : ScriptableObject
    {
        [field: Header("Movement Parameters")]
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public float Acceleration { get; private set; }


        [field: Header("Attack Parameters")]
        [field: SerializeField] public float PlayerDetectionRadius { get; private set; }

        [field: Header("Combat Parameters")]
        [field: SerializeField] public CombatData CombatData { get; private set; }

    }
}
