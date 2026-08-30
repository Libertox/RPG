using InputSystem.Enums;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputManager : IDisposable
    {
        public event Action OnMoveStarted;
        public event Action OnMoveEnded;

        public event Action OnAttackButtonPressed;
        public event Action OnInteractButtonPressed;
        public event Action OnInventoryButtonPressed;

        public event Action OnSubmitUIButtonPressed;
        public event Action OnContinueUIButtonPressed;
        public event Action OnCancelUIButtonPressed;

        public event Action OnLeftMouseClicked;
        public event Action OnRightMouseClicked;

        public event Action OnLeftMouseStartHolded;
        public event Action OnLeftMouseCancelHolded;

        private readonly InputActions _inputActions;

        public Vector2 MoveDirection => _inputActions.Player.Move.ReadValue<Vector2>();

        private bool isHoldLeftMouseButton;

        public InputManager()
        {
            _inputActions = new();

            _inputActions.Enable();

            EnableUIMap(false);

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

            _inputActions.UI.Click.canceled += OnLeftMouseButtonPerformed;
            _inputActions.UI.RightClick.performed += OnRightMouseButtonPerformed;
            _inputActions.UI.HoldLeftMouseButton.performed += OnHoldLeftMouseButtonStarted;
            _inputActions.UI.HoldLeftMouseButton.canceled += OnHoldLeftMouseButtonCanceled;

        }

        private void OnHoldLeftMouseButtonStarted(InputAction.CallbackContext action)
        {
            isHoldLeftMouseButton = true;

            OnLeftMouseStartHolded?.Invoke();
        }

        private void OnHoldLeftMouseButtonCanceled(InputAction.CallbackContext action)
        {
            isHoldLeftMouseButton = false;

            OnLeftMouseCancelHolded?.Invoke();
        }

        private void OnRightMouseButtonPerformed(InputAction.CallbackContext action)
        {
            if (isHoldLeftMouseButton) return;

            OnRightMouseClicked?.Invoke();
        }

        private void OnLeftMouseButtonPerformed(InputAction.CallbackContext action)
        {
            if (isHoldLeftMouseButton) return;

            OnLeftMouseClicked?.Invoke();
        }

        private void OnMoveButtonCanceled(InputAction.CallbackContext action)
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
            if (enable) _inputActions.Player.Enable();
            else _inputActions.Player.Disable();
        }

        public void EnableUIMap(bool enable = true)
        {
            if (enable) _inputActions.UI.Enable();
            else _inputActions.UI.Disable();
        }

        public static Vector2 GetMousePosition()
        {
            return Mouse.current.position.value;
        }

        public void Dispose()
        {
            _inputActions.Dispose();
        }

       
    }
}