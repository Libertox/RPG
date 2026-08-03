using System;

namespace DialogueSystem
{
    public class DialogueManager
    {
        public event Action OnDialogueStarted;
        public event Action OnDialogueCompleted;
        public event Action<DialogueLine> OnDialgoueLineChanged;

        private DialogueContainer _currentDialogue;
        private int _currentDialogueLine;

        public void StartDialogue(DialogueContainer dialogueContainer)
        {
            if (dialogueContainer == null) return;

            _currentDialogue = dialogueContainer;

            _currentDialogueLine = 0;

            OnDialogueStarted?.Invoke();

            OnDialgoueLineChanged?.Invoke(_currentDialogue.Dialogues[_currentDialogueLine]);       
        }

        public void ChangeDialogueLine()
        {
            _currentDialogueLine++;

            if (IsDialogueComplete())
            {
                OnDialogueCompleted?.Invoke();
                return;
            }

            OnDialgoueLineChanged?.Invoke(_currentDialogue.Dialogues[_currentDialogueLine]);
        }

        private bool IsDialogueComplete()
        {
            return _currentDialogueLine >= _currentDialogue.Dialogues.Length;
        }

    }
}
