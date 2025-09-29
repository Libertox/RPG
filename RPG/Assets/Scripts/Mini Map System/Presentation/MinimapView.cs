using Player;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MiniMapSystem.Presentation
{
    public class MinimapView : MonoBehaviour
    {
        [SerializeField] private RectTransform mapTexture;
        [SerializeField] private RectTransform playerIcon;

        [SerializeField] private MeshRenderer worldMap;

        [SerializeField] private MinimapIcon iconPrefab;

        private Vector2 _miniMapScale;
        private IMotionController _playerController;
        private MinimapController _minimapController;
        private MinimapIconPool _iconPool;

        private readonly Dictionary<IMinimapEntity, MinimapIcon> _staticIcons = new();
        private readonly Dictionary<IMinimapEntity, MinimapIcon> _dynamicIcons = new();

        [Inject]
        public void Construct(IMotionController playerController, MinimapController minimapController)
        {
            _playerController = playerController;
            _minimapController = minimapController;

            _minimapController.OnMinimapEntityAdded += OnMinimapEntityAdded;
            _minimapController.OnMinimapEntityRemoved += OnMinimapEntityRemoved;

            _iconPool = new(iconPrefab, mapTexture.transform);
        }

        private void Awake()
        {
            CalculateMapSize();
        }

        private void CalculateMapSize()
        {
            _miniMapScale = mapTexture.sizeDelta / new Vector2(worldMap.bounds.size.x, worldMap.bounds.size.z);
        }

        private void OnMinimapEntityRemoved(IMinimapEntity minimapEntity)
        {
            MinimapIcon icon;

            if (minimapEntity.IsStatic)
            {
                _staticIcons.TryGetValue(minimapEntity, out icon);
                _staticIcons.Remove(minimapEntity);
            }
            else
            {
                _dynamicIcons.TryGetValue(minimapEntity, out icon);
                _dynamicIcons.Remove(minimapEntity);
            }

            _iconPool.ReleaseMinimapIcon(icon);
        }

        private void OnMinimapEntityAdded(IMinimapEntity minimapEntity)
        {
            MinimapIcon icon = _iconPool.GetMinimapIcon()
                .SetIcon(minimapEntity.MarkerData.Icon)
                .SetSize(minimapEntity.MarkerData.Size)
                .SetPosition(minimapEntity.Position);

            if (minimapEntity.IsStatic)
                _staticIcons.Add(minimapEntity, icon);
            else
                _dynamicIcons.Add(minimapEntity, icon);
        }

        private void Update()
        {
            UpdateMapPosition();

            UpdateDynamicIconPosition();

            RotatePlayerIcon();
        }

        private void UpdateMapPosition()
        {
            var miniMapPosition = ConvertWorldPositionToMinimapPosition(-_playerController.Position);
            mapTexture.anchoredPosition = miniMapPosition;
        }

        private void UpdateDynamicIconPosition()
        {
            foreach (var dynamicIcon in _dynamicIcons)
                dynamicIcon.Value.SetPosition(ConvertWorldPositionToMinimapPosition(dynamicIcon.Key.Position));
        }

        private Vector2 ConvertWorldPositionToMinimapPosition(Vector3 worldPosition)
        {
            return new Vector2(worldPosition.x * _miniMapScale.x, worldPosition.z * _miniMapScale.y);
        }

        private void RotatePlayerIcon()
        {
            playerIcon.localEulerAngles = new Vector3(0f, 0f, -_playerController.Rotation.eulerAngles.y);
        }

        private void OnDestroy()
        {
            _minimapController.OnMinimapEntityAdded -= OnMinimapEntityAdded;
            _minimapController.OnMinimapEntityRemoved -= OnMinimapEntityRemoved;
        }
    }
}
