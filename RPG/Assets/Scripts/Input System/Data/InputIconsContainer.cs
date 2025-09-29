using InputSystem.Enums;
using InteractionPromptSystem;
using UnityEngine;
using Utility;

namespace InputSystem
{
    [CreateAssetMenu(fileName = nameof(InputIconsContainer), menuName = "ScriptableObjects/Input System/"+nameof(InputIconsContainer))]
    public class InputIconsContainer : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<PromptType, InputIcon> inputIcons;

        public Sprite GetInputIcons(PromptType promptType, ControllerType controllerType)
        {
            inputIcons.TryGetValue(promptType, out var icon);

            return icon.GetIcon(controllerType);
        }

    }

    [System.Serializable]
    public struct InputIcon
    {
        public Sprite PcIcon;
        public Sprite PsIcon;
        public Sprite XboxIcon;

        public readonly Sprite GetIcon(ControllerType controllerType)
        {
            return controllerType switch
            {
                ControllerType.PC => PcIcon,
                ControllerType.PSGamePad => PsIcon,
                ControllerType.XboxGamePad => XboxIcon,
                _ => null,
            };
        }
    }
}
