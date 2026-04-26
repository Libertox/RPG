

using InputSystem;
using InventorySystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Inventory
{
    public class ItemHolder : MonoBehaviour
    {
        [SerializeField] private Image icon;
        public ItemInventory HoldItem { get; private set; }

        private InputManager _inputManager;

        private IDragable _startSlot;

        [Inject]
        private void Construct(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

 
        public void SetItem(ItemInventory item, IDragable startSlot)
        {
            if(item == null) return;

            gameObject.SetActive(true);
            HoldItem = item;
            _startSlot = startSlot;
            icon.sprite = HoldItem.ItemBase.Icon;
        }

        public void ReturnToStartSlot()
        {
            _startSlot?.Drop();
            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            HoldItem = null;
        }

        public void Update()
        {
            transform.position = _inputManager.GetMousePosition();
        }
    }
}
