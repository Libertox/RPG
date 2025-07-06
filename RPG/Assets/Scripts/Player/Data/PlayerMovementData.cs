

using UnityEngine;

namespace Player.Data
{

    [CreateAssetMenu(fileName = nameof(PlayerMovementData), menuName = "ScriptableObjects/Plyaer/"+nameof(PlayerMovementData))]
    public class PlayerMovementData : ScriptableObject
    {
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }


    }
}
