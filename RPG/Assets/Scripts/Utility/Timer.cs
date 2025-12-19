using System;
using UnityEngine;

namespace Utility
{
    public class Timer
    {
        private readonly Action _onElapsed;
        private readonly Action _onStarted;

        private readonly float _destinationTime;

        private float _time;

        public bool IsRunning { get; private set; }

        public Timer(float destinationTime, Action onElapsed = null, Action onStarted = null)
        {
            _destinationTime = destinationTime;
            _onElapsed = onElapsed;
            _onStarted = onStarted;

            IsRunning = false;
        }

        public void Start()
        {
            _time = 0;
            IsRunning = true;
            _onStarted?.Invoke();
        }

        public void Tick()
        {
            if (!IsRunning) return;

            _time += Time.deltaTime;

            if(_time > _destinationTime)
            {
                _onElapsed?.Invoke();
                IsRunning = false;
            }
        }
    }
}
