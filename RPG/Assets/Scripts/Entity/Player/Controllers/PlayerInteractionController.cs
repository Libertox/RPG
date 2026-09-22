using InputSystem;
using InteractionSystem;
using UnityEngine;
using Zenject;

namespace Entity.Player
{
    public class PlayerInteractionController : MonoBehaviour, IInteractionController
    {
        private const int MAX_INTERACT = 10;

        [field: SerializeField] public LayerMask TargetLayerMask { get; private set; }
        [field: SerializeField] public float InteractionRange { get; private set; }

        private InputManager _inputManager;
        private PlayerController _playerController;

        private Collider[] _colliders;

        private void Awake()
        {
            _colliders = new Collider[MAX_INTERACT];

            _playerController = GetComponent<PlayerController>();
        }

        [Inject]
        public void Construct(InputManager inputManager)
        {
            _inputManager = inputManager;
            _inputManager.OnInteractPressed += TryInteractWithInteractableObject;
        }

        public void TryInteractWithInteractableObject()
        {
            int interactAmount = Physics.OverlapSphereNonAlloc(transform.position, InteractionRange, _colliders, TargetLayerMask);

            for(int i = 0;  i < interactAmount; i++)
            {
                if (_colliders[i].gameObject.TryGetComponent(out IInteractable interactable) || _colliders[i].transform.parent.TryGetComponent(out interactable))
                {
                    interactable.Interact(_playerController);
                }
            }
        }

        private void OnDestroy()
        {
            _inputManager.OnInteractPressed -= TryInteractWithInteractableObject;
        }
    }
}
