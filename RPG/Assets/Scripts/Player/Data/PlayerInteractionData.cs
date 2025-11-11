

using UnityEngine;

namespace Player.Data
{
    [CreateAssetMenu(fileName = nameof(PlayerInteractionData), menuName = "ScriptableObjects/Player/" + nameof(PlayerInteractionData))]
    public class PlayerInteractionData : ScriptableObject
    {
        [field: SerializeField] public LayerMask TargetLayerMask { get; private set; }
        [field: SerializeField] public float InteractionRange { get; private set; }

    }
}
