using UnityEngine;

namespace Entity.Player
{
    [CreateAssetMenu(fileName = "Player Data", menuName = "ScriptableObjects/Player/Player Data")]
    public class PlayerData : EntityData
    {
        [field: Header("Movement Data")]
        [field: SerializeField] public float EncumberedSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public float MaxLiftingCapacity { get; private set; }

    }
}
