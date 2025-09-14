using InputSystem;
using Zenject;
using UnityEngine;
using Player;
using MiniMapSystem;

namespace GameInitializers
{
    public class GameInitializer : MonoInstaller<GameInitializer>
    {
        [SerializeField] private PlayerController playerController;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();
            Container.BindInstance(playerController);
            Container.Bind<MinimapController>().AsSingle();
        }
    }
}
