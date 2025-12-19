using System;
using System.Collections;

namespace Entity
{
    public interface IAnimationController
    {
        public bool IsWaitingForEndAnimation { get; }

        public IEnumerator WaitForEndAnimation(string stateName, Action OnAnimationComplete = null, int layer = 0);
        public void SetIdleAnimation();
        public void SetMoveAnimation();
        public void SetAttackAnimation();
        public void SetGetHitAnimation();
        public void SetDieAnimation();
    }
}
