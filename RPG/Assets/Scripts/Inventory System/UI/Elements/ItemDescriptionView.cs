using InputSystem;
using InventorySystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace InventorySystem.UI
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

        public void Setup(InventorySlot item)
        {
            itemName.SetText(item.ItemBase.Name);
            description.SetText(item.ItemBase.Description);
        }

        public void ShowAtPosition(Vector3 worldPosition)
        {
            gameObject.SetActive(true);

            RectTransform rectTransform = (RectTransform)transform;

            RectTransform parentRect = rectTransform.parent as RectTransform;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                RectTransformUtility.WorldToScreenPoint(null, worldPosition),
                null,
                out Vector2 localPoint
            );

            rectTransform.anchoredPosition = localPoint;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
