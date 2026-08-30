using InventorySystem;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.UI
{
    public class ItemCategoryButton : MonoBehaviour
    {
        public event Action<ItemCategory> OnItemCategorySelected;

        [SerializeField] private ItemCategory itemCategory;

        [SerializeField] private Transform selector;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(SelectItemCategory);
        }

        private void SelectItemCategory()
        {
            OnItemCategorySelected?.Invoke(itemCategory);

            selector.transform.position = transform.position;
        }
    }
}
