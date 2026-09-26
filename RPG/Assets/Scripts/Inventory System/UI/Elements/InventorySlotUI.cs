using Entity.Player;
using InputSystem;
using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace InventorySystem.UI
{
    public class InventorySlotUI : UIElement<InventorySlotUI>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amount;
        [SerializeField] private Image background;
        [SerializeField] private GameObject paddlockIcon;

        public InventorySlot Item { get; private set; }

        private PlayerInventory playerInventory;
        private PlayerController playerController;
        private InputManager inputManager;
        private SelectInventorySlotSignal selectInventorySlotSignal;
        private DeselectInventorySlotSignal deselectInventorySlotSignal;

        [Inject]
        public void Construct(PlayerController playerController, InputManager inputManager, 
            SelectInventorySlotSignal selectInventorySlot, DeselectInventorySlotSignal deselectInventorySlot)
        {
            playerInventory = playerController.PlayerInventory;
            this.inputManager = inputManager;
            this.playerController = playerController;

            selectInventorySlotSignal = selectInventorySlot;
            deselectInventorySlotSignal = deselectInventorySlot;
        }

        public InventorySlotUI Initialize(InventorySlot item)
        {
            Item = item;

            icon.sprite = item.ItemBase.Icon;
            background.sprite = item.ItemBase.Rarity.Presentation;
            amount.SetText(item.Amount.ToString());
            amount.gameObject.SetActive(item.ItemBase.CanStack);

            paddlockIcon.SetActive(item.ItemBase.RequiredLevel > playerController.Statistic.Level.Value);

            return this;
        }

        private void EquipItem()
        {
            if (playerInventory.Equipment.TryEquipItem(Item))
            {
                deselectInventorySlotSignal.Fire();
            }
        }

        private void DropItem()
        {
            playerInventory.DropItem(Item);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!gameObject.activeSelf) return;

            selectInventorySlotSignal.Fire(this);

            inputManager.OnLeftMouseClicked += EquipItem;
            inputManager.OnRightMouseClicked += DropItem;
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            deselectInventorySlotSignal.Fire();

            inputManager.OnLeftMouseClicked -= EquipItem;
            inputManager.OnRightMouseClicked -= DropItem;
        }

        private void OnDisable()
        {
            inputManager.OnLeftMouseClicked -= EquipItem;
            inputManager.OnRightMouseClicked -= DropItem;
        }
    }
}
