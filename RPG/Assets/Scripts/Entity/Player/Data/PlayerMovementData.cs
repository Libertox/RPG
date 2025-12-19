

using UnityEngine;

namespace Entity.Player
{

    [CreateAssetMenu(fileName = nameof(PlayerMovementData), menuName = "ScriptableObjects/Player/"+nameof(PlayerMovementData))]
    public class PlayerMovementData : ScriptableObject
    {
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }


    }
}
