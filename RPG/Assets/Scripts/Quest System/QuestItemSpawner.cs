

using UnityEngine;
using Zenject;

namespace QuestSystem
{
    public class QuestItemSpawner : MonoBehaviour
    {
        [SerializeField] private ItemInteractable questItemPrefab;

        [SerializeField] private QuestStep questStep;

        private QuestItemFactory questItemFactory;

        [Inject]
        private void Construct(QuestItemFactory questItemFactory)
        {
            this.questItemFactory = questItemFactory;
        }
    
        private void Start()
        {
            questStep.OnStarted += SpawnItem;
        }

        private void SpawnItem()
        {
            questItemFactory.Create(questItemPrefab, transform.position);
        }
    }

    public class QuestItemFactory : Factory<ItemInteractable>
    {
        private DiContainer container;

        [Inject]
        private void Construct(DiContainer container)
        {
            this.container = container;
        }

        public ItemInteractable Create(ItemInteractable prefab, Vector3 position)
        {
            return container.InstantiatePrefabForComponent<ItemInteractable>(prefab, position, Quaternion.identity, null);
        }
    }
}
