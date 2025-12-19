

using UnityEngine;

namespace Entity.Player
{
    [CreateAssetMenu(fileName = nameof(PlayerData), menuName = "ScriptableObjects/Player/"+nameof(PlayerData))]
    public class PlayerData : ScriptableObject
    {
        [field: SerializeField] public CombatData CombatData { get; private set; }



    }
}
