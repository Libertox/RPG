using InputSystem;
using InteractionPromptSystem.Presentation;
using Player;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace InteractionPromptSystem
{
    public class InteractionPromptManager : MonoBehaviour
    {
        private PromptIconFactory _promptIconFactory;
        private PlayerController _playerMotionController;
        private InputManager _inputManager;

        private readonly List<IPromptProvider> _providers = new();
        private readonly Dictionary<IPromptProvider, PromptIcon> _icons = new();

        private readonly float _showPromptMaxDistance = 8f;
        private readonly float _showPromptMinDistance = 1f;

        private readonly float _interactionRange = 2f;

        private void Awake()
        {
            _promptIconFactory = GetComponent<PromptIconFactory>();
        }

        [Inject]
        private void Construct(PlayerController motionController, InputManager inputManager)
        {
            _playerMotionController = motionController;
            _inputManager = inputManager;
        }

        public void RegisterPromptProvider(IPromptProvider promptProvider)
        {
            _providers.Add(promptProvider);
        }

        public void UnregisterPromptProvider(IPromptProvider promptProvider)
        {
            RemovePrompt(promptProvider);

            _providers.Remove(promptProvider);
        }


        private void Update()
        {
            UpdateInteractionPrompts();
        }

        private void UpdateInteractionPrompts()
        {
            foreach (var provider in _providers)
            {
                float distance = Vector3.Distance(_playerMotionController.Position, provider.TargetPosition);
                bool withinPromptRange = distance < _showPromptMaxDistance;

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
            if (!_icons.TryGetValue(provider, out var icon))
            {
                icon = _promptIconFactory.Get();
                icon.transform.position = provider.PromptPosition;
                _icons.Add(provider, icon);
            }

            icon.SetVisibility(Mathf.InverseLerp(_showPromptMaxDistance, _showPromptMinDistance, distance));
            icon.LookAtCameraPosition();

            if (distance < _interactionRange)
            {
                icon.SetIcon(_inputManager.GetIconForPromptType(provider.Type));
            }
            else
            {
                icon.ResetIcon();
            }
        }

        private void RemovePrompt(IPromptProvider provider)
        {
            if (_icons.TryGetValue(provider, out var icon))
            {
                _promptIconFactory.Release(icon);
                _icons.Remove(provider);
            }
        }

    }

}
