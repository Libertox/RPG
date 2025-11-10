


using InputSystem;
using Item;
using UnityEngine;
using Zenject;

namespace InteractionPromptSystem
{
    public class PromptProvide : MonoBehaviour, IPromptProvider
    {
        [SerializeField] private PromptType promptType;
        [SerializeField] private Transform promptPosition;

        public PromptType Type => promptType;
        public Vector3 TargetPosition => transform.position;
        public Vector3 PromptPosition => promptPosition.position;

        private InteractionPromptManager _promptManager;

        private IInteractable _interactable;

        [Inject]
        public void Construct(InteractionPromptManager interactionPromptManager)
        {
            _promptManager = interactionPromptManager;
        }

        private void Start()
        {
            _promptManager.RegisterPromptProvider(this);

            _interactable = GetComponent<IInteractable>();
        }

        private void OnDestroy()
        {
            _promptManager.UnregisterPromptProvider(this);
        }

        public bool CanInteract()
        {
            return _interactable.CanInteract();
        }

    }
}
