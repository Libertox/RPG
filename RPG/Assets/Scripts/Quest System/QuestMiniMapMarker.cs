using UnityEngine;
using MiniMapSystem;
using Zenject;

namespace QuestSystem
{
    public class QuestMiniMapMarker : MinimapMarker
    {
        [SerializeField] private Quest quest;

        [SerializeField] private QuestStep questStep;

        private QuestManager questManager;

        [Inject]
        private void Construct(QuestManager questManager)
        {
            this.questManager = questManager;
        }

        protected override void Start()
        {
            base.Start();

            questStep.OnStarted += OnQuestStepStart;
            questStep.OnCompleted += OnQuestStepComplete;

            if (this.quest)
            {
                questManager.OnQuestStarted += OnQuestStarted;
                questManager.OnQuestCompleted += OnQuestCompleted;
            }
                
        }

        private void OnQuestCompleted(Quest quest)
        {
            if (this.quest == quest)
                Unregister();
        }

        private void OnQuestStarted(Quest quest)
        {
            if (this.quest == quest)
                Unregister();
        }

        private void OnQuestStepComplete()
        {
            Unregister();
        }

        private void OnQuestStepStart()
        {
            Register();
        }

        protected override void OnDestroy()
        {
            questStep.OnStarted -= OnQuestStepStart;
            questStep.OnCompleted -= OnQuestStepComplete;

            questManager.OnQuestStarted -= OnQuestStarted;

            base.OnDestroy();
        }
    }
}
