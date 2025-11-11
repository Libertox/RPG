

using UnityEngine;

namespace Player
{
    public interface IAnimationController
    {
        public Animator Animator { get; }

        public void SetIdleAnimation();
        public void SetMoveAnimation();
        public void SetAttackAnimation();
    }
}
