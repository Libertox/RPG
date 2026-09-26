

using UnityEngine;
using UnityEngine.Pool;

namespace InteractionPromptSystem.Presentation
{
    public class PromptIconFactory : MonoBehaviour
    {
        [SerializeField] private PromptIcon promptIconPrefab;
        [SerializeField] private Sprite basePromptIcon;

        [SerializeField] private float promptIconHeight;
        [SerializeField] private float promptIconWidth;

        private ObjectPool<PromptIcon> iconsPool;

        private void Awake()
        {
            iconsPool = new ObjectPool<PromptIcon>(OnCreateIcon, OnGetIcon, OnReleaseIcon);
        }

        public PromptIcon Get()
        {
            return iconsPool.Get();
        }

        public void Release(PromptIcon promptIcon)
        {
            if(promptIcon != null)
                iconsPool.Release(promptIcon);
        }

        private PromptIcon OnCreateIcon()
        {
            return Instantiate(promptIconPrefab)
                .SetBaseIcon(basePromptIcon)
                .SetHeight(promptIconHeight)
                .SetWidth(promptIconWidth);
        }

        private void OnGetIcon(PromptIcon promptIcon)
        {
            promptIcon.gameObject.SetActive(true);
        }

        private void OnReleaseIcon(PromptIcon promptIcon)
        {
            promptIcon.gameObject.SetActive(false);
        }


    }
}
