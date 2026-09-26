using Entity.Player;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace InventorySystem
{
    public class LootFactory : MonoBehaviour
    {
        [SerializeField] private Loot lootPrefab;

        private ObjectPool<Loot> lootPools;

        private Loot currentLoot;
        private PlayerController playerController;
        private DiContainer container;

        [Inject]
        private void Construct(PlayerController playerController, DiContainer container)
        {
            this.playerController = playerController;
            this.container = container;
         
        }

        private void Awake()
        {
            InitializePool();
        }

        private void Start()
        {
            playerController.PlayerInventory.OnItemDropped += OnItemDropped;
        }

        private void InitializePool()
        {
            lootPools = new ObjectPool<Loot>(OnCreateLoot, OnGetLoot, OnReleaseLoot);
        }

        private void OnGetLoot(Loot loot) => loot.gameObject.SetActive(true);
        private void OnReleaseLoot(Loot loot) => loot.gameObject.SetActive(false);
        private Loot OnCreateLoot() => container.InstantiatePrefabForComponent<Loot>(lootPrefab).Initalize(this);

        public void ReleaseLoot(Loot loot)
        {
            if (loot == currentLoot)
                currentLoot = null;

            lootPools.Release(loot);
        }

        private void OnItemDropped(InventorySlot item)
        {
            if (ShouldCreateNewLoot())
            {
                currentLoot = lootPools.Get();
                currentLoot.transform.position = playerController.transform.position;
            }

            currentLoot.AddItem(item);
                
        }

        private bool ShouldCreateNewLoot()
        {
            return currentLoot == null || Vector3.Distance(currentLoot.transform.position, playerController.transform.position) > 1f;
        }
    }
}
