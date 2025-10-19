using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        public event Action<Quest> OnQuestStarted;
        public event Action<Quest> OnQuestCompleted;

        [SerializeField] private Quest[] _quests;

        private Quest _activeQuest;

        private List<Quest> _availableQuests = new();
        private List<Quest> _finishedQuests = new();


        public void AddQuest(Quest newQuest)
        {
            _availableQuests.Add(newQuest);

            _activeQuest = newQuest;

            Debug.Log("Quest: " + newQuest.name + " Activated");

            newQuest.Start();

            OnQuestStarted?.Invoke(newQuest);
        }

        public void TryMoveToNextQuestStep()
        {
            if (_activeQuest.CanMoveToNextStep())
                _activeQuest.MoveToNextStep();
            else
                FinishQuest(_activeQuest);
        }


        public void FinishQuest(Quest finishedQuest)
        {
            _availableQuests.Remove(finishedQuest);

            Debug.Log("Quest: " + finishedQuest.name + " Finished");

            finishedQuest.SetQuestState(QuestState.Finished);

            _finishedQuests.Add(finishedQuest);

            OnQuestCompleted?.Invoke(finishedQuest);
        }

        private void OnDestroy()
        {
            foreach(var quest in _quests)
            {
                quest.Reset();
            }
        }

    }
}
