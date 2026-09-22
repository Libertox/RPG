using InputSystem.Enums;
using UnityEngine;
using System;
using Zenject;

namespace InputSystem
{
    public class InputDeviceChanger : ITickable
    {
        public Action<ControllerType> OnControllerChanged;

        private ControllerType _currentControllerType = ControllerType.PC;
        private ControllerType _lastControllerType;

        public ControllerType CurrentControllerType => _currentControllerType;

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
                        _currentControllerType = ControllerType.PlayStationGamepad;

                    if (_lastControllerType != _currentControllerType)
                    {
                        OnControllerChanged?.Invoke(_currentControllerType);
                    }

                    _lastControllerType = _currentControllerType;
                }
            }
        }
    }
}
