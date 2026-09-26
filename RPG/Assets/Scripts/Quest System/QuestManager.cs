using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        public event Action<Quest> OnQuestStarted;
        public event Action<Quest> OnQuestCompleted;

        public event Action<QuestStep> OnQuestStepChanged;

        [SerializeField] private Quest[] quests;

        private Quest activeQuest;

        private List<Quest> availableQuests = new();
        private List<Quest> finishedQuests = new();


        public void AddQuest(Quest newQuest)
        {
            availableQuests.Add(newQuest);

            activeQuest = newQuest;

            Debug.Log("Quest: " + newQuest.name + " Activated");

            newQuest.Start();

            OnQuestStarted?.Invoke(newQuest);

            OnQuestStepChanged?.Invoke(newQuest.CurrentQuestStep);
        }

        public void TryMoveToNextQuestStep()
        {
            if (activeQuest.CanMoveToNextStep())
            {
                activeQuest.MoveToNextStep();
                OnQuestStepChanged?.Invoke(activeQuest.CurrentQuestStep);
            }    
            else
                FinishQuest(activeQuest);
        }


        public void FinishQuest(Quest finishedQuest)
        {
            availableQuests.Remove(finishedQuest);

            Debug.Log("Quest: " + finishedQuest.name + " Finished");

            finishedQuest.SetQuestState(QuestState.Finished);

            finishedQuests.Add(finishedQuest);

            OnQuestCompleted?.Invoke(finishedQuest);
        }

        private void OnDestroy()
        {
            foreach(var quest in quests)
            {
                quest.Reset();
            }
        }

    }
}
