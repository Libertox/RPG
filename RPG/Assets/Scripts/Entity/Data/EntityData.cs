

using UnityEngine;

namespace Entity
{
    public class EntityData : ScriptableObject
    {
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float BaseHealth { get; private set; }
        [field: SerializeField] public int BaseLevel {  get; private set; }


        [field: Header("Combat Parameters")]
        [field: SerializeField] public CombatData CombatData { get; private set; }

    }
}
