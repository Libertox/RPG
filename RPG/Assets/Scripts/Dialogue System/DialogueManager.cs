using InputSystem;
using System;
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

        private InputManager _inputManager;

        [Inject]
        public void Construct(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

        public void StartDialogue(DialogueContainer dialogueContainer)
        {
            _currentDialogue = dialogueContainer;

            _currentDialogueLine = 0;

            OnDialogueStarted?.Invoke();

            _inputManager.EnableGameMap(false);

            OnDialgoueLineChanged?.Invoke(_currentDialogue.Dialogues[_currentDialogueLine]);
        }

        public void ChangeToNextDialogueLine()
        {
            _currentDialogueLine++;

            if (IsDialogueComplete())
            {
                _inputManager.EnableGameMap(true);
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
