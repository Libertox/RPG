
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniMapSystem
{
    public class MinimapController
    {
        public event Action<IMinimapEntity> OnMinimapEntityAdded;
        public event Action<IMinimapEntity> OnMinimapEntityRemoved;

        private readonly List<IMinimapEntity> minimapEntities;

        public MinimapController()
        {
            minimapEntities = new();
        }

        public void RegisterMinimapEntity(IMinimapEntity minimapEntity)
        {
            if (minimapEntities.Contains(minimapEntity)) return;

            minimapEntities.Add(minimapEntity);

            OnMinimapEntityAdded?.Invoke(minimapEntity);    
        }

        public void UnregisterMinimapEntity(IMinimapEntity minimapEntity)
        {
            if (!minimapEntities.Contains(minimapEntity)) return;

            minimapEntities.Remove(minimapEntity);

            OnMinimapEntityRemoved?.Invoke(minimapEntity);
        }

    }
}
