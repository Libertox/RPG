using MiniMapSystem;
using Entity.Player;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UI.MinimapView
{
    public class MinimapView : MonoBehaviour
    {
        [SerializeField] private RectTransform mapTexture;
        [SerializeField] private RectTransform playerIcon;

        [SerializeField] private MeshRenderer worldMap;

        [SerializeField] private MinimapIcon iconPrefab;

        private Vector2 miniMapScale;
        private PlayerController playerController;
        private MinimapController minimapController;
        private MinimapIconPool iconPool;

        private readonly Dictionary<IMinimapEntity, MinimapIcon> _staticIcons = new();
        private readonly Dictionary<IMinimapEntity, MinimapIcon> _dynamicIcons = new();

        [Inject]
        public void Construct(PlayerController playerController, MinimapController minimapController)
        {
            this.playerController = playerController;
            this.minimapController = minimapController;

            this.minimapController.OnMinimapEntityAdded += OnMinimapEntityAdded;
            this.minimapController.OnMinimapEntityRemoved += OnMinimapEntityRemoved;

            iconPool = new(iconPrefab, mapTexture.transform);
        }

        private void Awake()
        {
            CalculateMapSize();
        }

        private void CalculateMapSize()
        {
            miniMapScale = mapTexture.sizeDelta / new Vector2(worldMap.bounds.size.x, worldMap.bounds.size.z);
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

            iconPool.ReleaseMinimapIcon(icon);
        }

        private void OnMinimapEntityAdded(IMinimapEntity minimapEntity)
        {
            MinimapIcon icon = iconPool.GetMinimapIcon()
                .SetIcon(minimapEntity.MarkerData.Icon)
                .SetSize(minimapEntity.MarkerData.Size)
                .SetPosition(ConvertWorldPositionToMinimapPosition(minimapEntity.Position));

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
            var miniMapPosition = ConvertWorldPositionToMinimapPosition(-playerController.Position);
            mapTexture.anchoredPosition = miniMapPosition;
        }

        private void UpdateDynamicIconPosition()
        {
            foreach (var dynamicIcon in _dynamicIcons)
                dynamicIcon.Value.SetPosition(ConvertWorldPositionToMinimapPosition(dynamicIcon.Key.Position));
        }

        private Vector2 ConvertWorldPositionToMinimapPosition(Vector3 worldPosition)
        {
            return new Vector2(worldPosition.x * miniMapScale.x, worldPosition.z * miniMapScale.y);
        }

        private void RotatePlayerIcon()
        {
            playerIcon.localEulerAngles = new Vector3(0f, 0f, -playerController.Rotation.eulerAngles.y);
        }

        private void OnDestroy()
        {
            minimapController.OnMinimapEntityAdded -= OnMinimapEntityAdded;
            minimapController.OnMinimapEntityRemoved -= OnMinimapEntityRemoved;
        }
    }
}
