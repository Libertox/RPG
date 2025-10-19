
using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = nameof(Quest), menuName = "ScriptableObjects/Quest System/"+nameof(Quest))]
    public class Quest : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public QuestStep[] QuestSteps { get; private set; }

        private QuestState _questState;
        public int CurrentStep { get; private set; } = 0;
        public QuestStep CurrentQuestStep => QuestSteps[CurrentStep];

        public void Start()
        {
            SetQuestState(QuestState.InProgress);

            QuestSteps[CurrentStep].StartStep();
        }

        public void MoveToNextStep()
        {
            QuestSteps[CurrentStep].FinishStep();

            CurrentStep++;

            QuestSteps[CurrentStep].StartStep();
        }

        public void SetQuestState(QuestState questState)
        {
            _questState = questState;
        }

        public bool CanMoveToNextStep()
        {
            return CurrentStep < QuestSteps.Length - 1;
        }

        public bool IsInactive() => _questState == QuestState.Inactive;
        public bool IsInProgress() => _questState == QuestState.InProgress;
        public bool IsAllStepCompleted() => _questState == QuestState.AllStepsCompleted;
        public bool IsFinished() => _questState == QuestState.Finished;


        public void Reset()
        {
            _questState = QuestState.Inactive;

            CurrentStep = 0;

            foreach (var questStep in QuestSteps)
            {
                questStep.Reset();
            }
        }

    }

   
}
