using InputSystem;
using UnityEngine;
using Zenject;

namespace InteractionPromptSystem
{
    public class PromptProvide : MonoBehaviour, IPromptProvider
    {
        [SerializeField] private InputIconContainer promptIcon;
        [SerializeField] private Transform promptPosition;

        public InputIconContainer Icon => promptIcon;
        public Vector3 TargetPosition => transform.position;
        public Vector3 PromptPosition => promptPosition.position;

        private InteractionPromptManager _promptManager;

        private IInteractablePrompt _interactable;

        [Inject]
        public void Construct(InteractionPromptManager interactionPromptManager)
        {
            _promptManager = interactionPromptManager;
        }

        private void Start()
        {
            _promptManager.RegisterPromptProvider(this);

            _interactable = GetComponent<IInteractablePrompt>();
        }

        private void OnDestroy()
        {
            _promptManager.UnregisterPromptProvider(this);
        }

        public bool CanInteract()
        {
            return gameObject.activeSelf && _interactable.CanInteract();
        }

    }
}
