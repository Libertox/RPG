using InputSystem;
using Zenject;

namespace GameInitializers
{
    public class GameInitializer : MonoInstaller<GameInitializer>
    {
        private InputManager inputManager;

        public override void InstallBindings()
        {
            inputManager = new InputManager();

            Container.BindInstance(inputManager);
        }

        private void OnDestroy()
        {
            inputManager.Dispose();
        }

    }
}
