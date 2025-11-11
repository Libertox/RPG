

using UnityEngine;

namespace Player
{
    public interface IAnimationController
    {
        public AnimatorStateInfo GetCurrentAnimatorStateInfo();
        public void SetIdleAnimation();
        public void SetMoveAnimation();
        public void SetAttackAnimation();
    }
}
