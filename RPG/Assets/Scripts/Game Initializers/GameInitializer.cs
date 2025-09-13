using InputSystem;
using Zenject;

namespace GameInitializers
{
    public class GameInitializer : MonoInstaller<GameInitializer>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();
        }
    }
}
