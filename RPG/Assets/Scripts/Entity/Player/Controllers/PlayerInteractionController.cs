using InputSystem;
using Item;
using UnityEngine;
using Zenject;

namespace Entity.Player
{
    public class PlayerInteractionController : MonoBehaviour, IInteractionController
    {
        private const int MAX_INTERACT = 10;

        [SerializeField] private PlayerInteractionData interactionData;

        private InputManager _inputManager;

        private Collider[] _colliders;

        private void Awake()
        {
            _colliders = new Collider[MAX_INTERACT];
        }

        [Inject]
        public void Construct(InputManager inputManager)
        {
            _inputManager = inputManager;
            _inputManager.OnInteractButtonPressed += TryInteractWithInteractableObject;
        }

        public void TryInteractWithInteractableObject()
        {
            int interactAmount = Physics.OverlapSphereNonAlloc(transform.position, interactionData.InteractionRange, _colliders, interactionData.TargetLayerMask);

            for(int i = 0;  i < interactAmount; i++)
            {
                if (_colliders[i].gameObject.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact();
                }
            }
        }

        private void OnDestroy()
        {
            _inputManager.OnInteractButtonPressed -= TryInteractWithInteractableObject;
        }
    }
}
