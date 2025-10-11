using InputSystem.Enums;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace InputSystem
{
    public class InputManager: IDisposable, ITickable
    {
        public Action<ControllerType> OnControllerChanged;
        public InputEvents InputEvents { get; private set; }

        private readonly InputActions _inputActions;
        private readonly InputIconsContainer _iconsContainer;

        private ControllerType _currentControllerType = ControllerType.PC;
        private ControllerType _lastControllerType;

        public InputManager(InputIconsContainer inputIconsContainer)
        {
            _inputActions = new();
            InputEvents = new();
            _iconsContainer = inputIconsContainer;

            _inputActions.Enable();

            SubscribeInputAction();
        }

        private void SubscribeInputAction()
        {
            _inputActions.Player.Attack.performed += OnAttackButtonPerformed;
            _inputActions.Player.Interact.performed += OnInteractButtonPerformed;

            _inputActions.UI.Submit.performed += OnSubmitButtonPerformed;
            _inputActions.UI.Continue.performed += OnContinueButtonPerformed;
        }

        private void OnContinueButtonPerformed(InputAction.CallbackContext action)
        {
            InputEvents.InvokeOnContinueButtonPressed();
        }

        private void OnSubmitButtonPerformed(InputAction.CallbackContext action)
        {
            InputEvents.InvokeOnSubmitButtonPressed();
        }

        private void OnInteractButtonPerformed(InputAction.CallbackContext action)
        {
            InputEvents.InvokeOnInteractButtonPressed();
        }

        private void OnAttackButtonPerformed(InputAction.CallbackContext action)
        {
            InputEvents.InvokeOnAttackButtonPressed();
        }

        public void EnableGameMap(bool enable = true)
        {
            if(enable) _inputActions.Player.Enable();
            else _inputActions.Player.Disable();
        }

        public void EnableUIMap(bool enable = true)
        {
            if (enable) _inputActions.UI.Enable();
            else _inputActions.UI.Disable();
        }

        public Sprite GetIconForPromptType(PromptType promptType)
        {
            return _iconsContainer.GetInputIcons(promptType, _currentControllerType);
        }

        public void Tick()
        {
            HandleMoveAction();

            SetActiveController();
        }

        private void SetActiveController()
        {
            foreach (var device in UnityEngine.InputSystem.InputSystem.devices)
            {
                if (device.wasUpdatedThisFrame)
                {
                    if (device.displayName == "Mouse" || device.displayName == "Keyboard")
                        _currentControllerType = ControllerType.PC;
                    else
                        _currentControllerType = ControllerType.PSGamePad;

                    if(_lastControllerType != _currentControllerType)
                    {
                        OnControllerChanged?.Invoke(_currentControllerType);
                    }
                        
                    _lastControllerType = _currentControllerType;
                }
            }
        }

        private void HandleMoveAction()
        {
            var moveActionInput = _inputActions.Player.Move.ReadValue<Vector2>();

            InputEvents.InvokeOnMoveButtonPressed(moveActionInput);
        }


        public void Dispose()
        {
            _inputActions.Dispose();
        }

       
    }
}