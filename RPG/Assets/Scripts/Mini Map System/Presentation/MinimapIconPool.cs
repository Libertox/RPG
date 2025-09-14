using UnityEngine;
using UnityEngine.Pool;

namespace MiniMapSystem.Presentation
{
    public class MinimapIconPool
    {
        private readonly MinimapIcon _minimapIconPrefab;

        private readonly ObjectPool<MinimapIcon> _iconsPool;

        private readonly Transform _iconsParent;

        public MinimapIconPool(MinimapIcon minimapIconPrefab, Transform iconsParent)
        {
            _minimapIconPrefab = minimapIconPrefab;
            _iconsParent = iconsParent;

            _iconsPool = new ObjectPool<MinimapIcon>(CreateMinimapIcon, OnMinimapIconGet, OnMinimapIconReleased);
        }

        private MinimapIcon CreateMinimapIcon()
        {
            return Object.Instantiate(_minimapIconPrefab, _iconsParent);
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
            return _iconsPool.Get();
        } 

        public void ReleaseMinimapIcon(MinimapIcon minimapIcon)
        {
            _iconsPool.Release(minimapIcon);
        }

    }
}
