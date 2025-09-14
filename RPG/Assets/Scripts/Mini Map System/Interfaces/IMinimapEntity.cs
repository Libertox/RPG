

using MiniMapSystem.Data;
using UnityEngine;

namespace MiniMapSystem
{
    public interface IMinimapEntity
    {
        public bool IsStatic { get; }
        public MinimapMarkerData MarkerData { get; }
        public Vector3 Position { get; }

    }
}
