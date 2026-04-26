using InputSystem;
using InventorySystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.Inventory
{
    public class ItemDescriptionView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI description;

        [SerializeField] private float showDelay;

        private InputManager _inputManager;

        [Inject]
        private void Construct(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

        public  void Setup(ItemInventory item)
        {
            itemName.SetText(item.ItemBase.Name);
            description.SetText(item.ItemBase.Description);

            Show();
        }

        private void Show()
        {
            //await Awaitable.WaitForSecondsAsync(showDelay);

            gameObject.SetActive(true);

            RectTransform rectTransform = (RectTransform)transform;

            rectTransform.anchoredPosition = _inputManager.GetMousePosition();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
