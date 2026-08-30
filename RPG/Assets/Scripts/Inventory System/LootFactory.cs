using Entity.Player;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace InventorySystem
{
    public class LootFactory : MonoBehaviour
    {
        [SerializeField] private Loot lootPrefab;

        private ObjectPool<Loot> _lootPools;

        private Loot _currentLoot;
        private PlayerController _playerController;
        private DiContainer _container;

        [Inject]
        private void Construct(PlayerController playerController, DiContainer container)
        {
            _playerController = playerController;
            _container = container;
         
        }

        private void Awake()
        {
            InitializePool();
        }

        private void Start()
        {
            _playerController.PlayerInventory.OnItemDropped += OnItemDropped;
        }

        private void InitializePool()
        {
            _lootPools = new ObjectPool<Loot>(OnCreateLoot, OnGetLoot, OnReleaseLoot);
        }

        private void OnGetLoot(Loot loot) => loot.gameObject.SetActive(true);
        private void OnReleaseLoot(Loot loot) => loot.gameObject.SetActive(false);
        private Loot OnCreateLoot() => _container.InstantiatePrefabForComponent<Loot>(lootPrefab).Initalize(this);

        public void ReleaseLoot(Loot loot)
        {
            if (loot == _currentLoot)
                _currentLoot = null;

            _lootPools.Release(loot);
        }

        private void OnItemDropped(InventoryItem item)
        {
            if (ShouldCreateNewLoot())
            {
                _currentLoot = _lootPools.Get();
                _currentLoot.transform.position = _playerController.transform.position;
            }

            _currentLoot.AddItem(item);
                
        }

        private bool ShouldCreateNewLoot()
        {
            return _currentLoot == null || Vector3.Distance(_currentLoot.transform.position, _playerController.transform.position) > 1f;
        }
    }
}
