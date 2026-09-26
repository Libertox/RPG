

using QuestSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.QuestView
{
    public class QuestView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI questName;
        [SerializeField] private TextMeshProUGUI questStepName;

        private QuestManager questManager;

        [Inject]
        private void Construct(QuestManager questManager)
        {
            this.questManager = questManager;

            this.questManager.OnQuestCompleted += OnQuestCompleted;
            this.questManager.OnQuestStarted += OnQuestStarted;
            this.questManager.OnQuestStepChanged += OnQuestStepChanged;
        }

        private void OnQuestStepChanged(QuestStep questStep)
        {
            UpdateQuestStepName(questStep.StepName);
        }

        private void OnQuestStarted(Quest quest)
        {
            Show();
            UpdateQuestName(quest.Name);
        }

        private void OnQuestCompleted(Quest quest)
        {
            Hide();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void UpdateQuestName(string questName)
        {
            this.questName.SetText(questName);
        }

        private void UpdateQuestStepName(string questStepName)
        {
            this.questStepName.SetText(questStepName);
        }

        private void OnDestroy()
        {
            questManager.OnQuestCompleted -= OnQuestCompleted;
            questManager.OnQuestStarted -= OnQuestStarted;
            questManager.OnQuestStepChanged -= OnQuestStepChanged;
        }
    }

}
