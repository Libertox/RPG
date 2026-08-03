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
        private PromptIconFactory _promptIconFactory;
        private PlayerController _playerMotionController;
        private InputDeviceChanger _inputDeviceChanger;

        [SerializeField] private float _showPromptMaxDistance = 8f;
        [SerializeField] private float _showPromptMinDistance = 1f;

        [SerializeField] private float _interactionRange = 2f;

        private readonly List<IPromptProvider> _providers = new();
        private readonly Dictionary<IPromptProvider, PromptIcon> _icons = new();

        private void Awake()
        {
            _promptIconFactory = GetComponent<PromptIconFactory>();
        }

        [Inject]
        private void Construct(PlayerController motionController, InputDeviceChanger inputDeviceChanger)
        {
            _playerMotionController = motionController;
            _inputDeviceChanger = inputDeviceChanger;
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
                icon.SetIcon(provider.Icon.GetInputIcons(_inputDeviceChanger.CurrentControllerType));
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
