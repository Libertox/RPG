using Item;
using UnityEngine;
using Zenject;

namespace QuestSystem
{
    public class ItemInteractable : MonoBehaviour, IInteractable
    {
        private QuestManager _questManager;

        [Inject]
        private void Construct(QuestManager questManager)
        {
            _questManager = questManager;
        }
  
        public bool CanInteract()
        {
            return true;
        }

        public void Interact()
        {
            if (!CanInteract()) return;

            _questManager.TryMoveToNextQuestStep();

            Destroy(gameObject);
        }
    }
}
