using InputSystem;
using Item;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerInteractionController : MonoBehaviour, IInteractionController
    {
        private const int MAX_INTERACT = 10;

        private InputEvents _inputEvents;

        private Collider[] _colliders;

        private void Awake()
        {
            _colliders = new Collider[MAX_INTERACT];
        }

        [Inject]
        public void Construct(InputManager inputManager)
        {
            _inputEvents = inputManager.InputEvents;
            _inputEvents.OnInteractButtonPressed += TryInteractWithInteractableObject;
        }

        public void TryInteractWithInteractableObject()
        {
            int interactAmount = Physics.OverlapSphereNonAlloc(transform.position, 2f, _colliders, LayerMask.NameToLayer("Interactable"));

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
            _inputEvents.OnInteractButtonPressed -= TryInteractWithInteractableObject;
        }
    }
}
