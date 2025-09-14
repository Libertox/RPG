
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniMapSystem
{
    public class MinimapController
    {
        public event Action<IMinimapEntity> OnMinimapEntityAdded;
        public event Action<IMinimapEntity> OnMinimapEntityRemoved;

        public MinimapController()
        {
            _minimapEntities = new();
        }

        private List<IMinimapEntity> _minimapEntities;

        public List<IMinimapEntity> MinimapEntities => _minimapEntities;

        public void RegisterMinimapEntity(IMinimapEntity minimapEntity)
        {
            _minimapEntities.Add(minimapEntity);

            OnMinimapEntityAdded?.Invoke(minimapEntity);    
        }

        public void UnregisterMinimapEntity(IMinimapEntity minimapEntity)
        {
            _minimapEntities.Remove(minimapEntity);

            OnMinimapEntityRemoved?.Invoke(minimapEntity);
        }

    }
}
