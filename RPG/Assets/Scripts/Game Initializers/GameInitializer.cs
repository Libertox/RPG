using InputSystem;
using Zenject;
using UnityEngine;
using Player;
using MiniMapSystem;
using InteractionPromptSystem;
using DialogueSystem;
using QuestSystem;
using UI;

namespace GameInitializers
{
    public class GameInitializer : MonoInstaller<GameInitializer>
    {
        [SerializeField] private PlayerController playerController;

        [SerializeField] private InteractionPromptManager interactionPromptManager;

        [SerializeField] private InputIconsContainer iconsContainer;

        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private QuestManager questManager;
        [SerializeField] private UIViewManager viewManager;

        [SerializeField] private GameObject itemPrefab;
         
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle().WithArguments(iconsContainer);

            Container.BindInstance(playerController);
            Container.BindInstance(interactionPromptManager).AsSingle();
            Container.BindInstance(dialogueManager).AsSingle();
            Container.BindInstance(questManager).AsSingle();
            Container.BindInstance(viewManager).AsSingle();

            Container.Bind<MinimapController>().AsSingle();

            Container.BindFactory<ItemInteractable, QuestItemFactory>().FromComponentInNewPrefab(itemPrefab);
        }
    }
}
