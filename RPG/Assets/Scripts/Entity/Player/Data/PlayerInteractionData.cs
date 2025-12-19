

using UnityEngine;

namespace Entity.Player
{
    [CreateAssetMenu(fileName = nameof(PlayerInteractionData), menuName = "ScriptableObjects/Player/" + nameof(PlayerInteractionData))]
    public class PlayerInteractionData : ScriptableObject
    {
        [field: SerializeField] public LayerMask TargetLayerMask { get; private set; }
        [field: SerializeField] public float InteractionRange { get; private set; }

    }
}
