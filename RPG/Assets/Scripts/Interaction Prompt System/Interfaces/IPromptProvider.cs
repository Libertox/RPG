using InputSystem;
using UnityEngine;

namespace InteractionPromptSystem 
{
    public interface IPromptProvider
    {
        public InputIconContainer Icon { get; }
        public Vector3 TargetPosition { get; }
        public Vector3 PromptPosition { get; }

        public bool CanInteract();

    }
}


