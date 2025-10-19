using System;
using UI;
using UI.DialogueView;
using UnityEngine;
using Zenject;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        public event Action OnDialogueStarted;
        public event Action OnDialogueCompleted;

        public event Action<DialogueLine> OnDialgoueLineChanged;

        private DialogueContainer _currentDialogue;

        private int _currentDialogueLine;

        private UIViewManager _viewManager;

        [Inject]
        public void Construct(UIViewManager viewManager)
        {
            _viewManager = viewManager;
        }

        public async void StartDialogue(DialogueContainer dialogueContainer)
        {
            _currentDialogue = dialogueContainer;

            _currentDialogueLine = 0;

            OnDialogueStarted?.Invoke();

            await _viewManager.TryOpenView<DialogueView>(true);

            OnDialgoueLineChanged?.Invoke(_currentDialogue.Dialogues[_currentDialogueLine]);       
        }

        public void ChangeToNextDialogueLine()
        {
            _currentDialogueLine++;

            if (IsDialogueComplete())
            {
                OnDialogueCompleted?.Invoke();
                return;
            }

            OnDialgoueLineChanged?.Invoke(_currentDialogue.Dialogues[_currentDialogueLine]);
        }

        public bool IsDialogueComplete()
        {
            return _currentDialogueLine >= _currentDialogue.Dialogues.Length;
        }

    }
}
