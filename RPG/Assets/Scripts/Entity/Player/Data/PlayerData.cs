

using InventorySystem;
using UnityEngine;

namespace Entity.Player
{
    [System.Serializable]
    public class PlayerData
    {
        [field: Header("Movement Data")]
        [SerializeField] private float movementSpeed;
        [SerializeField] private float encumberedSpeed;
        [field: SerializeField] public float RotationSpeed { get; private set; }

        [field: SerializeField] public CombatData CombatData { get; private set; }

        [SerializeField] private float maxLiftingCapacity;

        public PlayerInventory Inventory { get; private set; }

        public PlayerData()
        {
            Inventory = new PlayerInventory();
        }

        public float GetMovementSpeed()
        {
            float speed = Inventory.LiftingCapacity >= maxLiftingCapacity ? encumberedSpeed : movementSpeed;

            return speed;
        }


    }
}
