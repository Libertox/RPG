using UnityEngine;

namespace Entity.Enemy
{
    [CreateAssetMenu(fileName = nameof(EnemyData), menuName = "ScriptableObjects/Enemy/"+nameof(EnemyData))]
    public class EnemyData : EntityData
    {
        [field: Header("Movement Parameters")]
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public float Acceleration { get; private set; }

        [field: Header("Attack Parameters")]
        [field: SerializeField] public float PlayerDetectionRadius { get; private set; }


    }
}
