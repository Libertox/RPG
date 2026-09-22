using InputSystem.Enums;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputManager : IDisposable
    {
        public event Action OnMoveStarted;
        public event Action OnMoveCanceled;

        public event Action OnAttackPressed;
        public event Action OnInteractPressed;
        public event Action OnInventoryPressed;

        public event Action OnSubmitPressed;
        public event Action OnContinuePressed;
        public event Action OnCancelPressed;

        public event Action OnCompareHoldStarted;
        public event Action OnCompareHoldCanceled;

        public event Action OnSortItemsPressed;

        public event Action OnLeftMouseClicked;
        public event Action OnRightMouseClicked;

        public event Action OnLeftMouseHoldStarted;
        public event Action OnLeftMouseHoldCanceled;

        private readonly InputActions _inputActions;

        public Vector2 MoveDirection => _inputActions.Player.Move.ReadValue<Vector2>();

        private bool isHoldLeftMouseButton;

        public InputManager()
        {
            _inputActions = new();

            _inputActions.Enable();

            EnableUIActions(false);

            SubscribeInputActions();
        }

        private void SubscribeInputActions()
        {
            _inputActions.Player.Move.performed += OnMoveButtonStarted;
            _inputActions.Player.Move.canceled += OnMoveButtonCanceled;

            _inputActions.Player.Attack.performed += OnAttackButtonPerformed;
            _inputActions.Player.Interact.performed += OnInteractButtonPerformed;

            _inputActions.Player.Inventory.performed += OnInventoryButtonPerformed;

            _inputActions.UI.Submit.performed += OnSubmitButtonPerformed;
            _inputActions.UI.Continue.performed += OnContinueButtonPerformed;
            _inputActions.UI.Cancel.performed += OnCancelButtonPerformed;

            _inputActions.UI.Click.canceled += OnLeftMouseClickCanceled;
            _inputActions.UI.RightClick.performed += OnRightMouseButtonPerformed;
            _inputActions.UI.HoldLeftMouseButton.performed += OnHoldLeftMouseButtonStarted;
            _inputActions.UI.HoldLeftMouseButton.canceled += OnHoldLeftMouseButtonCanceled;

            _inputActions.UI.Compare.started += OnHoldCompareButtonStarted;
            _inputActions.UI.Compare.canceled += OnHoldCompareButtonEnded;

            _inputActions.UI.SortItems.started += OnSortItemsButtonPerformed;
        }

        private void OnSortItemsButtonPerformed(InputAction.CallbackContext action)
        {
            OnSortItemsPressed?.Invoke();
        }

        private void OnHoldCompareButtonEnded(InputAction.CallbackContext action)
        {
            OnCompareHoldCanceled?.Invoke();
        }

        private void OnHoldCompareButtonStarted(InputAction.CallbackContext action)
        {
            OnCompareHoldStarted?.Invoke();
        }

        public bool IsCompareButtonPressed()
        {
            return _inputActions.UI.Compare.IsPressed();
        }

        private void OnHoldLeftMouseButtonStarted(InputAction.CallbackContext action)
        {
            isHoldLeftMouseButton = true;

            OnLeftMouseHoldStarted?.Invoke();
        }

        private void OnHoldLeftMouseButtonCanceled(InputAction.CallbackContext action)
        {
            isHoldLeftMouseButton = false;

            OnLeftMouseHoldCanceled?.Invoke();
        }

        private void OnRightMouseButtonPerformed(InputAction.CallbackContext action)
        {
            if (isHoldLeftMouseButton) return;

            OnRightMouseClicked?.Invoke();
        }

        private void OnLeftMouseClickCanceled(InputAction.CallbackContext action)
        {
            if (isHoldLeftMouseButton) return;

            OnLeftMouseClicked?.Invoke();
        }

        private void OnMoveButtonCanceled(InputAction.CallbackContext action)
        {
            OnMoveCanceled?.Invoke();
        }

        private void OnMoveButtonStarted(InputAction.CallbackContext action)
        {
            OnMoveStarted?.Invoke();
        }

        private void OnContinueButtonPerformed(InputAction.CallbackContext action)
        {
            OnContinuePressed?.Invoke();
        }

        private void OnSubmitButtonPerformed(InputAction.CallbackContext action)
        {
            OnSubmitPressed?.Invoke();
        }

        private void OnCancelButtonPerformed(InputAction.CallbackContext action)
        {
            OnCancelPressed?.Invoke();
        }

        private void OnInteractButtonPerformed(InputAction.CallbackContext action)
        {
            OnInteractPressed?.Invoke();
        }

        private void OnAttackButtonPerformed(InputAction.CallbackContext action)
        {
            OnAttackPressed?.Invoke();
        }

        private void OnInventoryButtonPerformed(InputAction.CallbackContext action)
        {
            OnInventoryPressed?.Invoke();
        }

        public void EnablePlayerActions(bool enable = true)
        {
            if (enable) _inputActions.Player.Enable();
            else _inputActions.Player.Disable();
        }

        public void EnableUIActions(bool enable = true)
        {
            if (enable) _inputActions.UI.Enable();
            else _inputActions.UI.Disable();
        }

        public static Vector2 GetMousePosition()
        {
            return Mouse.current.position.value;
        }

        private void UnsubscribeInputActions()
        {
            _inputActions.Player.Move.performed -= OnMoveButtonStarted;
            _inputActions.Player.Move.canceled -= OnMoveButtonCanceled;

            _inputActions.Player.Attack.performed -= OnAttackButtonPerformed;
            _inputActions.Player.Interact.performed -= OnInteractButtonPerformed;

            _inputActions.Player.Inventory.performed -= OnInventoryButtonPerformed;

            _inputActions.UI.Submit.performed -= OnSubmitButtonPerformed;
            _inputActions.UI.Continue.performed -= OnContinueButtonPerformed;
            _inputActions.UI.Cancel.performed -= OnCancelButtonPerformed;

            _inputActions.UI.Click.canceled -= OnLeftMouseClickCanceled;
            _inputActions.UI.RightClick.performed -= OnRightMouseButtonPerformed;
            _inputActions.UI.HoldLeftMouseButton.performed -= OnHoldLeftMouseButtonStarted;
            _inputActions.UI.HoldLeftMouseButton.canceled -= OnHoldLeftMouseButtonCanceled;

            _inputActions.UI.Compare.started -= OnHoldCompareButtonStarted;
            _inputActions.UI.Compare.canceled -= OnHoldCompareButtonEnded;

            _inputActions.UI.SortItems.started -= OnSortItemsButtonPerformed;
        }

        public void Dispose()
        {
            UnsubscribeInputActions();

            _inputActions.Disable();
            _inputActions.Dispose();
        }

       
    }
}