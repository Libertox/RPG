using UnityEngine;
using UnityEngine.Pool;

namespace UI.MinimapView
{
    public class MinimapIconPool
    {
        private readonly MinimapIcon minimapIconPrefab;

        private readonly ObjectPool<MinimapIcon> iconsPool;

        private readonly Transform iconsParent;

        public MinimapIconPool(MinimapIcon minimapIconPrefab, Transform iconsParent)
        {
            this.minimapIconPrefab = minimapIconPrefab;
            this.iconsParent = iconsParent;

            iconsPool = new ObjectPool<MinimapIcon>(CreateMinimapIcon, OnMinimapIconGet, OnMinimapIconReleased);
        }

        private MinimapIcon CreateMinimapIcon()
        {
            return Object.Instantiate(minimapIconPrefab, iconsParent);
        }

        private void OnMinimapIconGet(MinimapIcon minimapIcon)
        {
            minimapIcon.gameObject.SetActive(true);
        }

        private void OnMinimapIconReleased(MinimapIcon minimapIcon)
        {
            minimapIcon.gameObject.SetActive(false);
        }

        public MinimapIcon GetMinimapIcon()
        {
            return iconsPool.Get();
        } 

        public void ReleaseMinimapIcon(MinimapIcon minimapIcon)
        {
            iconsPool.Release(minimapIcon);
        }

    }
}
