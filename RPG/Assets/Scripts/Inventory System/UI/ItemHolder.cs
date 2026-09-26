using InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.UI
{
    public class ItemHolder : MonoBehaviour
    {
        [SerializeField] private Image icon;
        public InventorySlot HoldItem { get; private set; }

        private IItemContainer startSlot;

        public void Setup(InventorySlot item, IItemContainer startSlot)
        {
            if(item == null) return;

            gameObject.SetActive(true);
            HoldItem = item;
            this.startSlot = startSlot;
            SetIcon(HoldItem.ItemBase.Icon);
        }

        private void SetIcon(Sprite icon)
        {
            this.icon.sprite = icon;
        }

        public void ReturnToStartSlot()
        {
            startSlot?.Drop(HoldItem);
            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            HoldItem = null;
        }

        public void Update()
        {
            transform.position = InputManager.GetMousePosition();
        }
    }
}
