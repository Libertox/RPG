using System;

namespace DialogueSystem
{
    public class DialogueManager
    {
        public event Action OnDialogueStarted;
        public event Action OnDialogueCompleted;
        public event Action<DialogueLine> OnDialgoueLineChanged;

        private DialogueContainer currentDialogue;
        private int currentDialogueLine;

        public void StartDialogue(DialogueContainer dialogueContainer)
        {
            if (dialogueContainer == null) return;

            currentDialogue = dialogueContainer;

            currentDialogueLine = 0;

            OnDialogueStarted?.Invoke();

            OnDialgoueLineChanged?.Invoke(currentDialogue.Dialogues[currentDialogueLine]);       
        }

        public void ChangeDialogueLine()
        {
            currentDialogueLine++;

            if (IsDialogueComplete())
            {
                OnDialogueCompleted?.Invoke();
                return;
            }

            OnDialgoueLineChanged?.Invoke(currentDialogue.Dialogues[currentDialogueLine]);
        }

        private bool IsDialogueComplete()
        {
            return currentDialogueLine >= currentDialogue.Dialogues.Length;
        }

    }
}
