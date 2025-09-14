

using MiniMapSystem.Data;
using UnityEngine;
using Zenject;

namespace MiniMapSystem
{
    public class MinimapMarker : MonoBehaviour, IMinimapEntity
    {
        [SerializeField] private bool isStaticMarker;

        [SerializeField] private MinimapMarkerData markerData;

        public bool IsStatic => isStaticMarker;
        public MinimapMarkerData MarkerData => markerData;
        public Vector3 Position => transform.position;


        private MinimapController _minimapController;

        [Inject]
        public void Construct(MinimapController minimapController)
        {
            _minimapController = minimapController;
        }

        private void Start()
        {
            _minimapController.RegisterMinimapEntity(this);
        }

        private void OnDestroy()
        {
            _minimapController?.UnregisterMinimapEntity(this);
        }

    }
}
