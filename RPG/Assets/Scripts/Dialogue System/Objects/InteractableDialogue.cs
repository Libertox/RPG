using Entity.Player;
using Item;
using UnityEngine;
using Zenject;

namespace DialogueSystem
{
    public class InteractableDialogue : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueContainer dialogue;

        private DialogueManager _dialogueManager;

        [Inject]
        public void Construct(DialogueManager dialogueManager)
        {
            _dialogueManager = dialogueManager;
        }

        public bool CanInteract()
        {
            return true;
        }

        public void Interact(PlayerController playerController)
        {
            _dialogueManager.StartDialogue(dialogue);
        }
    }

}

