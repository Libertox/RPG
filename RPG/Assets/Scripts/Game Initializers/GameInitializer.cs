using InputSystem;
using Zenject;
using UnityEngine;
using Player;
using MiniMapSystem;
using InteractionPromptSystem;
using DialogueSystem;

namespace GameInitializers
{
    public class GameInitializer : MonoInstaller<GameInitializer>
    {
        [SerializeField] private PlayerMotionController playerController;

        [SerializeField] private InteractionPromptManager interactionPromptManager;

        [SerializeField] private InputIconsContainer iconsContainer;

        [SerializeField] private DialogueManager dialogueManager;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle().WithArguments(iconsContainer);

            Container.BindInstance<IMotionController>(playerController);
            Container.BindInstance(interactionPromptManager);
            Container.BindInstance(dialogueManager);

            Container.Bind<MinimapController>().AsSingle();
        }
    }
}
