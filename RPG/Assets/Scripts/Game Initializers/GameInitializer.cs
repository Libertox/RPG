using InputSystem;
using Zenject;
using UnityEngine;
using MiniMapSystem;
using InteractionPromptSystem;
using DialogueSystem;
using QuestSystem;
using UI;
using InventorySystem.UI;

namespace Initializers
{
    public class GameInitializer : MonoInstaller<GameInitializer>
    {
        [SerializeField] private InteractionPromptManager interactionPromptManager;

        [SerializeField] private QuestManager questManager;
        [SerializeField] private UIViewManager viewManager;

        [SerializeField] private GameObject itemPrefab;

        [SerializeField] private GameObject slotPrefab;

        public override void InstallBindings()
        {
            Container.DeclareSignal<DeselectInventorySlotSignal>();
            Container.DeclareSignal<SelectInventorySlotSignal>();

            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();

            Container.BindInstance(interactionPromptManager).AsSingle(); 
            Container.BindInstance(questManager).AsSingle();
            Container.BindInstance(viewManager).AsSingle();

            Container.Bind<MinimapController>().AsSingle();
            Container.Bind<DialogueManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputDeviceChanger>().AsSingle();

            Container.BindFactory<ItemInteractable, QuestItemFactory>().FromComponentInNewPrefab(itemPrefab);

            Container.BindMemoryPool<InventorySlotUI, InventoryItemSlotPool>().FromComponentInNewPrefab(slotPrefab);

        }
    }
}
