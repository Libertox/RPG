using DialogueSystem;
using Entity.Player;
using InteractionSystem;
using UnityEngine;
using Utility.Attribute;
using Zenject;

namespace QuestSystem
{
    public class NPCQuestGiver : InteractionBase, IInteractable
    {
        [Header("Quest Refernces")]
        [SerializeField] private Quest quest;

        [SerializeField] private QuestStep questStep;

        [Header("Dialogue Refernces")]
        [SerializeField] private DialogueContainer dialogueOnQuestStart;
        [SerializeField] private DialogueContainer dialogueOnQuestEnd;

        [SerializeField] private bool enableMidQuestDialogue;
        [SerializeField, ShowIf(nameof(enableMidQuestDialogue))] private DialogueContainer dialogueDuringQuest;


        private DialogueManager _dialogueManager;
        private QuestManager _questManager;

        [Inject]
        private void Construct(DialogueManager dialogueManager, QuestManager questManager)
        {
            _dialogueManager = dialogueManager;
            _questManager = questManager;
        }

        public override bool CanInteract()
        {
            return !quest.IsFinished();
        }

        public override void Execute(PlayerController playerController)
        {
            TryInvokeDialogueOnQuestStart();
            TryInvokeDialogueDuringQuest();
            TryInvokeDialogueOnQuestEnd();
        }

        private void TryInvokeDialogueOnQuestStart()
        {
            if(quest.IsInactive())
            {
                _dialogueManager.StartDialogue(dialogueOnQuestStart);
                _dialogueManager.OnDialogueCompleted += ActiveQeust;
            }
        }

        private void TryInvokeDialogueDuringQuest()
        {
            if (quest.IsInProgress() && enableMidQuestDialogue && !questStep.IsActive)
            {
                _dialogueManager.StartDialogue(dialogueDuringQuest);
            }
        }

        private void TryInvokeDialogueOnQuestEnd()
        {
            if (questStep.IsActive)
            {
                _dialogueManager.StartDialogue(dialogueOnQuestEnd);

                _dialogueManager.OnDialogueCompleted += FinishQuest;
            }
        }

        private void FinishQuest()
        {
            _questManager.TryMoveToNextQuestStep();
            _dialogueManager.OnDialogueCompleted -= FinishQuest;
        }

        private void ActiveQeust()
        {
            _questManager.AddQuest(quest);
            _dialogueManager.OnDialogueCompleted -= ActiveQeust;
        }
    }
}
