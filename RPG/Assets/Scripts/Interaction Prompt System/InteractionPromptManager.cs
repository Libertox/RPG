using InputSystem;
using InteractionPromptSystem.Presentation;
using Entity.Player;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace InteractionPromptSystem
{
    public class InteractionPromptManager : MonoBehaviour
    {
        private PromptIconFactory promptIconFactory;
        private PlayerController playerMotionController;
        private InputDeviceChanger inputDeviceChanger;

        [SerializeField] private float showPromptMaxDistance = 8f;
        [SerializeField] private float showPromptMinDistance = 1f;

        [SerializeField] private float interactionRange = 2f;

        private readonly List<IPromptProvider> providers = new();
        private readonly Dictionary<IPromptProvider, PromptIcon> icons = new();

        private void Awake()
        {
            promptIconFactory = GetComponent<PromptIconFactory>();
        }

        [Inject]
        private void Construct(PlayerController motionController, InputDeviceChanger inputDeviceChanger)
        {
            playerMotionController = motionController;
            this.inputDeviceChanger = inputDeviceChanger;
        }

        public void RegisterPromptProvider(IPromptProvider promptProvider)
        {
            providers.Add(promptProvider);
        }

        public void UnregisterPromptProvider(IPromptProvider promptProvider)
        {
            RemovePrompt(promptProvider);

            providers.Remove(promptProvider);
        }


        private void Update()
        {
            UpdateInteractionPrompts();
        }

        private void UpdateInteractionPrompts()
        {
            foreach (var provider in providers)
            {
                float distance = Vector3.Distance(playerMotionController.Position, provider.TargetPosition);
                bool withinPromptRange = distance < showPromptMaxDistance;

                if (withinPromptRange && provider.CanInteract())
                {
                    HandlePrompt(provider, distance);
                }
                else
                {
                    RemovePrompt(provider);
                }
            }
        }

        private void HandlePrompt(IPromptProvider provider, float distance)
        {
            if (!icons.TryGetValue(provider, out var icon))
            {
                icon = promptIconFactory.Get();
                icon.transform.position = provider.PromptPosition;
                icons.Add(provider, icon);
            }

            icon.SetVisibility(Mathf.InverseLerp(showPromptMaxDistance, showPromptMinDistance, distance));
            icon.LookAtCameraPosition();

            if (distance < interactionRange)
            {
                icon.SetIcon(provider.Icon.GetInputIcons(inputDeviceChanger.CurrentControllerType));
            }
            else
            {
                icon.ResetIcon();
            }
        }

        private void RemovePrompt(IPromptProvider provider)
        {
            if (icons.TryGetValue(provider, out var icon))
            {
                promptIconFactory.Release(icon);
                icons.Remove(provider);
            }
        }

    }

}
