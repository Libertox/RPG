using InputSystem.Enums;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace InputSystem
{
    public class InputManager : IDisposable, ITickable
    {
        public event Action OnMoveStarted;
        public event Action OnMoveEnded;

        public event Action OnAttackButtonPressed;
        public event Action OnInteractButtonPressed;
        public event Action OnInventoryButtonPressed;

        public event Action OnSubmitUIButtonPressed;
        public event Action OnContinueUIButtonPressed;
        public event Action OnCancelUIButtonPressed;

        public Action<ControllerType> OnControllerChanged;

        private readonly InputActions _inputActions;
        private readonly InputIconsContainer _iconsContainer;

        private ControllerType _currentControllerType = ControllerType.PC;
        private ControllerType _lastControllerType;

        public Vector2 MoveDirection => _inputActions.Player.Move.ReadValue<Vector2>();

        public InputManager(InputIconsContainer inputIconsContainer)
        {
            _inputActions = new();
            _iconsContainer = inputIconsContainer;

            _inputActions.Enable();

            SubscribeInputAction();
        }

        private void SubscribeInputAction()
        {
            _inputActions.Player.Move.performed += OnMoveButtonStarted;
            _inputActions.Player.Move.canceled += OnMoveButtonCanceled;

            _inputActions.Player.Attack.performed += OnAttackButtonPerformed;
            _inputActions.Player.Interact.performed += OnInteractButtonPerformed;

            _inputActions.Player.Inventory.performed += OnInventoryButtonPerformed;

            _inputActions.UI.Submit.performed += OnSubmitButtonPerformed;
            _inputActions.UI.Continue.performed += OnContinueButtonPerformed;
            _inputActions.UI.Cancel.performed += OnCancelButtonPerformed;

        }
        private void OnMoveButtonCanceled(InputAction.CallbackContext obj)
        {
            OnMoveEnded?.Invoke();
        }

        private void OnMoveButtonStarted(InputAction.CallbackContext action)
        {
            OnMoveStarted?.Invoke();
        }

        private void OnContinueButtonPerformed(InputAction.CallbackContext action)
        {
            OnContinueUIButtonPressed?.Invoke();
        }

        private void OnSubmitButtonPerformed(InputAction.CallbackContext action)
        {
            OnSubmitUIButtonPressed?.Invoke();
        }

        private void OnCancelButtonPerformed(InputAction.CallbackContext action)
        {
            OnCancelUIButtonPressed?.Invoke();
        }

        private void OnInteractButtonPerformed(InputAction.CallbackContext action)
        {
            OnInteractButtonPressed?.Invoke();
        }

        private void OnAttackButtonPerformed(InputAction.CallbackContext action)
        {
            OnAttackButtonPressed?.Invoke();
        }

        private void OnInventoryButtonPerformed(InputAction.CallbackContext action)
        {
            OnInventoryButtonPressed?.Invoke();
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
        public void Dispose()
        {
            _inputActions.Dispose();
        }

       
    }
}