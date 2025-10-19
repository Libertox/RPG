

using MiniMapSystem.Data;
using UnityEngine;
using Zenject;

namespace MiniMapSystem
{
    public class MinimapMarker : MonoBehaviour, IMinimapEntity
    {
        [SerializeField] private bool registerMarkerOnAwake = true;

        [SerializeField] private bool isStaticMarker;

        [SerializeField] private MinimapMarkerData markerData;

        public bool IsStatic => isStaticMarker;
        public MinimapMarkerData MarkerData => markerData;
        public Vector3 Position => transform.position;
        public MinimapController MinimapController => _minimapController;



        private MinimapController _minimapController;

        [Inject]
        public void Construct(MinimapController minimapController)
        {
            _minimapController = minimapController;
        }

        protected virtual void Start()
        {
            if(registerMarkerOnAwake)
                _minimapController.RegisterMinimapEntity(this);
        }

        protected virtual void OnDestroy()
        {
            _minimapController?.UnregisterMinimapEntity(this);
        }

    }
}
