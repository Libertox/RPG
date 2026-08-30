using InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.UI
{
    public class ItemHolder : MonoBehaviour
    {
        [SerializeField] private Image icon;
        public InventoryItem HoldItem { get; private set; }

        private IItemContainer _startSlot;

        public void SetItem(InventoryItem item, IItemContainer startSlot)
        {
            if(item == null) return;

            gameObject.SetActive(true);
            HoldItem = item;
            _startSlot = startSlot;
            icon.sprite = HoldItem.ItemBase.Icon;
        }

        public void ReturnToStartSlot()
        {
            _startSlot?.Drop(HoldItem);
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
