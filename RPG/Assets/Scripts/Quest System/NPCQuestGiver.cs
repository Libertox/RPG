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

        private DialogueManager dialogueManager;
        private QuestManager questManager;

        [Inject]
        private void Construct(DialogueManager dialogueManager, QuestManager questManager)
        {
            this.dialogueManager = dialogueManager;
            this.questManager = questManager;
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
                dialogueManager.StartDialogue(dialogueOnQuestStart);
                dialogueManager.OnDialogueCompleted += ActiveQeust;
            }
        }

        private void TryInvokeDialogueDuringQuest()
        {
            if (quest.IsInProgress() && enableMidQuestDialogue && !questStep.IsActive)
            {
                dialogueManager.StartDialogue(dialogueDuringQuest);
            }
        }

        private void TryInvokeDialogueOnQuestEnd()
        {
            if (questStep.IsActive)
            {
                dialogueManager.StartDialogue(dialogueOnQuestEnd);

                dialogueManager.OnDialogueCompleted += FinishQuest;
            }
        }

        private void FinishQuest()
        {
            questManager.TryMoveToNextQuestStep();
            dialogueManager.OnDialogueCompleted -= FinishQuest;
        }

        private void ActiveQeust()
        {
            questManager.AddQuest(quest);
            dialogueManager.OnDialogueCompleted -= ActiveQeust;
        }
    }
}
