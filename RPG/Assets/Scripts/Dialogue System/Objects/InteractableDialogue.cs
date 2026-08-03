using Entity.Player;
using InteractionSystem;
using UnityEngine;
using Zenject;

namespace DialogueSystem
{
    public class InteractableDialogue : InteractionBase, IInteractable
    {
        [SerializeField] private DialogueContainer dialogue;

        private DialogueManager _dialogueManager;

        [Inject]
        public void Construct(DialogueManager dialogueManager)
        {
            _dialogueManager = dialogueManager;
        }

        public override void Execute(PlayerController playerController)
        {
            base.Execute(playerController);

            _dialogueManager.StartDialogue(dialogue);
        }
    }

}

