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

        private InteractionPromptManager promptManager;

        private IInteractablePrompt interactable;

        [Inject]
        public void Construct(InteractionPromptManager interactionPromptManager)
        {
            promptManager = interactionPromptManager;
        }

        private void Start()
        {
            promptManager.RegisterPromptProvider(this);

            interactable = GetComponent<IInteractablePrompt>();
        }

        private void OnDestroy()
        {
            promptManager.UnregisterPromptProvider(this);
        }

        public bool CanInteract()
        {
            return gameObject.activeSelf && interactable.CanInteract();
        }

    }
}
