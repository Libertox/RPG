using Entity.Player;
using InteractionSystem;
using UnityEngine;
using Zenject;

namespace DialogueSystem
{
    public class InteractableDialogue : InteractionBase, IInteractable
    {
        [SerializeField] private DialogueContainer dialogue;

        private DialogueManager dialogueManager;

        [Inject]
        public void Construct(DialogueManager dialogueManager)
        {
            this.dialogueManager = dialogueManager;
        }

        public override void Execute(PlayerController playerController)
        {
            base.Execute(playerController);

            dialogueManager.StartDialogue(dialogue);
        }
    }

}

