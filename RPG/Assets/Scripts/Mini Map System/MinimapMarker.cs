

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
        public bool IsSelected => isSelected;

        private MinimapController minimapController;
        private bool isSelected; 

        [Inject]
        public void Construct(MinimapController minimapController)
        {
            this.minimapController = minimapController;
        }

        protected virtual void Start()
        {
            if (registerMarkerOnAwake)
                Register();
        }

        public virtual void Register()
        {
            minimapController.RegisterMinimapEntity(this);
        }

        public void Unregister()
        {
            minimapController?.UnregisterMinimapEntity(this);

            Deselect();
        }

        public void Select()
        {
            isSelected = true;
        }

        public void Deselect()
        {
            isSelected = false;
        }

        protected virtual void OnDestroy()
        {
            Unregister();
        }

    

    }
}
