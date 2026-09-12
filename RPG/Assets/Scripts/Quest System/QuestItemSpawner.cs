

using UnityEngine;
using Zenject;

namespace QuestSystem
{
    public class QuestItemSpawner : MonoBehaviour
    {
        [SerializeField] private ItemInteractable questItemPrefab;

        [SerializeField] private QuestStep questStep;

        private QuestItemFactory _questItemFactory;

        [Inject]
        private void Construct(QuestItemFactory questItemFactory)
        {
            _questItemFactory = questItemFactory;
        }
    
        private void Start()
        {
            questStep.OnStarted += SpawnItem;
        }

        private void SpawnItem()
        {
            _questItemFactory.Create(questItemPrefab, transform.position);
        }
    }

    public class QuestItemFactory : Factory<ItemInteractable>
    {
        private DiContainer _container;

        [Inject]
        private void Construct(DiContainer container)
        {
            _container = container;
        }

        public ItemInteractable Create(ItemInteractable prefab, Vector3 position)
        {
            return _container.InstantiatePrefabForComponent<ItemInteractable>(prefab, position, Quaternion.identity, null);
        }
    }
}
