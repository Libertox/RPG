using Entity.Player;
using InventorySystem;
using UnityEngine;
using Zenject;

namespace Initializers
{
    public class PlayerInitializer : MonoInstaller
    {
        [SerializeField] private PlayerController playerController;

        [SerializeField] private InventorySettings playerInventorySettings;

        public override void InstallBindings()
        {
            Container.Bind<PlayerInventory>().AsSingle().WithArguments(playerInventorySettings);

            Container.BindInstance(playerController).AsSingle();
        }

    }
}
