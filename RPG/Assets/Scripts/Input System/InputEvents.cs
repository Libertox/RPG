using System;
using UnityEngine;

namespace InputSystem
{
    public class InputEvents
    {
        public event Action<Vector2> OnMoveButtonPressed;
        public event Action OnAttackButtonPressed;


        public void InvokeOnMoveButtonPressed(Vector2 moveInput)
        {
            OnMoveButtonPressed?.Invoke(moveInput);
        }

        public void InvokeOnAttackButtonPressed()
        {
            OnAttackButtonPressed?.Invoke();
        }

    }
}
