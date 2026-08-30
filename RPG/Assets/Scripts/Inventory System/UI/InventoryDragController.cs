using InputSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace InventorySystem.UI
{
    public class InventoryDragController : MonoBehaviour
    {
        [SerializeField] private GraphicRaycaster raycaster;
        [SerializeField] private EventSystem eventSystem;

        [SerializeField] private ItemHolder itemHolder;

        private InputManager _inputManager;

        [Inject]
        private void Construct(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

        private void Start()
        {
            _inputManager.OnLeftMouseStartHolded += OnLeftMouseStartHolded;
            _inputManager.OnLeftMouseCancelHolded += OnLeftMouseCancelHolded;
        }

        private void OnLeftMouseCancelHolded()
        {
            var itemContainer = GetSlotUnderMouse(InputManager.GetMousePosition());

            if (itemContainer == null)
            {
                itemHolder.ReturnToStartSlot();
                return;
            }

            if (!itemContainer.Drop(itemHolder.HoldItem))
            {
                itemHolder.ReturnToStartSlot();
            }

            itemHolder.Hide();
        }

        private void OnLeftMouseStartHolded()
        {
            var itemContainer = GetSlotUnderMouse(InputManager.GetMousePosition());

            if (itemContainer == null) return;

            itemHolder.SetItem(itemContainer.Get(), itemContainer);
        }

        public IItemContainer GetSlotUnderMouse(Vector2 mousePos)
        {
            PointerEventData data = new(eventSystem);
            data.position = mousePos;

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(data, results);

            foreach (var r in results)
            {
                if (r.gameObject.TryGetComponent<IItemContainer>(out var slot))
                    return slot;
            }

            return null;
        }
    }
}
