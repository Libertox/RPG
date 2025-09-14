

using UnityEngine;

namespace MiniMapSystem.Data
{
    [CreateAssetMenu(fileName = nameof(MinimapMarkerData), menuName = "ScriptableObjects/MiniMapSystem/" + nameof(MinimapMarkerData))]
    public class MinimapMarkerData : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public Vector2 Size { get; private set; }

    }
}
