using Entity.Player;
using MiniMapSystem;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using Utility;
using Zenject;

namespace UI.MinimapView
{
    public class MinimapView : MonoBehaviour
    {
        [SerializeField] private RectTransform mapTexture;
        [SerializeField] private RectTransform playerIcon;

        [SerializeField] private MeshRenderer worldMap;
        [SerializeField] private MinimapIcon iconPrefab;

        [SerializeField] private TextMeshProUGUI targetDistanceText;
        [SerializeField] private MinimapIcon selectedMarkerIcon;
        [SerializeField] private RectTransform minimapCenter;
        [SerializeField] private float minmapRadious;

        private Vector2 miniMapScale;
        private PlayerController playerController;
        private MinimapController minimapController;
        private ComponentPool<MinimapIcon> iconPool;
        private bool hasSelectedIcon;

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

            iconPool.Release(icon);

            if (minimapEntity.IsSelected)
                hasSelectedIcon = IsAnyIconSelected();
        }

        private bool IsAnyIconSelected()
        {
            return GetSelectedMinimapEntity() != null;
        }

        private IMinimapEntity GetSelectedMinimapEntity()
        {
            foreach (var icon in _staticIcons)
            {
                if (icon.Key.IsSelected)
                    return icon.Key;
            }

            foreach (var icon in _dynamicIcons)
            {
                if (icon.Key.IsSelected)
                    return icon.Key;
            }

            return null;
        }

        private void OnMinimapEntityAdded(IMinimapEntity minimapEntity)
        {
            MinimapIcon icon = CreateMinimapIcon(minimapEntity);

            if (minimapEntity.IsStatic)
                _staticIcons.Add(minimapEntity, icon);
            else
                _dynamicIcons.Add(minimapEntity, icon);

            if (minimapEntity.IsSelected)
            {
                hasSelectedIcon = true;
                selectedMarkerIcon.SetIcon(minimapEntity.MarkerData.Icon);
                selectedMarkerIcon.SetSize(minimapEntity.MarkerData.Size);
            }
        }

        private MinimapIcon CreateMinimapIcon(IMinimapEntity minimapEntity)
        {
            return iconPool.Get()
                .SetIcon(minimapEntity.MarkerData.Icon)
                .SetSize(minimapEntity.MarkerData.Size)
                .SetPosition(ConvertWorldPositionToMinimapPosition(minimapEntity.Position));
        }

        private void Update()
        {
            UpdateMapPosition();
            UpdateDynamicIconPosition();
            UpdateSelectIconPosition();

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

        private void UpdateSelectIconPosition()
        {
            selectedMarkerIcon.gameObject.SetActive(hasSelectedIcon);
            targetDistanceText.gameObject.SetActive(hasSelectedIcon);

            if (!hasSelectedIcon) return;

            var selectedMinimapIcon = GetSelectedMinimapEntity();

            if (selectedMinimapIcon == null) return;

            Vector3 worldDirection = selectedMinimapIcon.Position - playerController.Position;

            Vector2 minimapDirection = new Vector2(worldDirection.x * miniMapScale.x, worldDirection.z * miniMapScale.y);

            UpdateTargetDistanceText(minimapDirection.magnitude);

            if (minimapDirection.magnitude < minmapRadious)
            {
                selectedMarkerIcon.gameObject.SetActive(false);
                return;
            }

            if (minimapDirection.sqrMagnitude < 0.001f) return;

            Vector2 markerPosition = minimapDirection.normalized * minmapRadious;

            selectedMarkerIcon.transform.position = minimapCenter.TransformPoint(new Vector3(markerPosition.x, markerPosition.y, 0f));
        }

        private void UpdateTargetDistanceText(float distance)
        {
            targetDistanceText.SetText(distance.ToString("F0"));
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

        private void OnDrawGizmos()
        {
            if (!minimapCenter) return;

            Handles.DrawWireDisc(minimapCenter.position, Vector3.forward, minmapRadious);
        }
    }
}
