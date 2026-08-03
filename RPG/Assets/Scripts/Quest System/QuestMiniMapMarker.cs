using UnityEngine;
using MiniMapSystem;
using Zenject;

namespace QuestSystem
{
    public class QuestMiniMapMarker : MinimapMarker
    {
        [SerializeField] private Quest quest;

        [SerializeField] private QuestStep questStep;

        private QuestManager _questManager;

        [Inject]
        private void Construct(QuestManager questManager)
        {
            _questManager = questManager;
        }

        protected override void Start()
        {
            base.Start();

            questStep.OnStarted += OnQuestStepStart;
            questStep.OnCompleted += OnQuestStepComplete;

            if (this.quest)
            {
                _questManager.OnQuestStarted += OnQuestStarted;
                _questManager.OnQuestCompleted += OnQuestCompleted;
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

            _questManager.OnQuestStarted -= OnQuestStarted;

            base.OnDestroy();
        }
    }
}
