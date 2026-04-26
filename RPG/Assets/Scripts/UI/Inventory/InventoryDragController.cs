using InputSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UI.Inventory
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
            var dragable = GetSlotUnderMouse(_inputManager.GetMousePosition());

            if (dragable == null)
            {
                itemHolder.ReturnToStartSlot();
                return;
            }

            dragable?.Drop();

            itemHolder.Hide();
        }

        private void OnLeftMouseStartHolded()
        {
            var dragable = GetSlotUnderMouse(_inputManager.GetMousePosition());

            dragable?.Drag();
        }

        public IDragable GetSlotUnderMouse(Vector2 mousePos)
        {
            PointerEventData data = new(eventSystem);
            data.position = mousePos;

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(data, results);

            foreach (var r in results)
            {
                if (r.gameObject.TryGetComponent<IDragable>(out var slot))
                    return slot;
            }

            return null;
        }
    }
}
