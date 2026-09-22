using InputSystem;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace InventorySystem.UI
{
    public class ItemDescriptionView : MonoBehaviour
    {
        [SerializeField] private ItemInformationUI selectedItemInformation;

        [SerializeField] private ItemInformationUI equippedItemInformation;

        private SelectInventorySlotSignal selectInventorySlotSignal;
        private DeselectInventorySlotSignal deselectInventorySlotSignal;
        private InputManager inputManager;
        private PlayerInventory playerInventory;

        private ItemConfigBase selectedItem;

        [Inject]
        private void Construct(SelectInventorySlotSignal selectInventorySlotSignal, 
            DeselectInventorySlotSignal deselectInventorySlotSignal, 
            InputManager inputManager,
            PlayerInventory playerInventory)
        {
            this.selectInventorySlotSignal = selectInventorySlotSignal;
            this.deselectInventorySlotSignal = deselectInventorySlotSignal;
            this.inputManager = inputManager;
            this.playerInventory = playerInventory;

            this.selectInventorySlotSignal.Listen(OnItemSlotSelected);
            this.deselectInventorySlotSignal.Listen(OnItemSlotDeselected);
        }

        private void OnItemSlotSelected(InventorySlotUI slot)
        {
            selectedItem = slot.Item.ItemBase;

            selectedItemInformation.ShowAtPosition(slot.GetRightBottomCornerPosition());
            selectedItemInformation.Setup(selectedItem);

            selectedItemInformation.ActiveComparisonItemLabel(CanShowEquippedItemInformation());

            LayoutRebuilder.ForceRebuildLayoutImmediate(selectedItemInformation.transform as RectTransform);

            inputManager.OnCompareHoldStarted += ShowEquippedItemInformation;
            inputManager.OnCompareHoldCanceled += HideEquippedItemInformation;

            if (inputManager.IsCompareButtonPressed())
                ShowEquippedItemInformation();
        }

        private void ShowEquippedItemInformation()
        {
            if (!CanShowEquippedItemInformation()) return;

            var equippedItem = playerInventory.Equipment.GetEquipped(selectedItem.EquipmentSlot, 0);

            equippedItemInformation.ShowAtPosition(selectedItemInformation.GetRightTopCornerPosition());
            equippedItemInformation.Setup(equippedItem.ItemBase);

            var equippedItemComparable = equippedItem.ItemBase as IComparableItem;
            var selectedItemComparable = selectedItem as IComparableItem;

            equippedItemInformation.ActiveComparisonResult(true);
            selectedItemInformation.ActiveComparisonResult(true);

            equippedItemInformation.UpdateComparisonResult(equippedItemComparable.ComparisonValue - selectedItemComparable.ComparisonValue);
            selectedItemInformation.UpdateComparisonResult(selectedItemComparable.ComparisonValue - equippedItemComparable.ComparisonValue);

            LayoutRebuilder.ForceRebuildLayoutImmediate(equippedItemInformation.transform as RectTransform);
        }

        private bool CanShowEquippedItemInformation()
        {
            return selectedItem is IComparableItem 
                && playerInventory.Equipment.GetEquipped(selectedItem.EquipmentSlot, 0) != null;
        }

        private void HideEquippedItemInformation()
        {
            equippedItemInformation.Hide();

            equippedItemInformation.ActiveComparisonResult(false);
            selectedItemInformation.ActiveComparisonResult(false);
        }

        private void OnItemSlotDeselected()
        {
            equippedItemInformation.Hide();
            selectedItemInformation.Hide();

            inputManager.OnCompareHoldStarted -= ShowEquippedItemInformation;
            inputManager.OnCompareHoldCanceled -= HideEquippedItemInformation;
        }
            

        private void OnDestroy()
        {
            selectInventorySlotSignal.Unlisten(OnItemSlotSelected);
            deselectInventorySlotSignal.Unlisten(OnItemSlotDeselected);
        }
    }
}
