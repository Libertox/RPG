using System;

namespace Entity
{
    [DefaultController(typeof(DefaultAnimationController))]
    public interface IAnimationController : IController
    {
        public bool IsWaitingForEndAnimation { get; }

        public void WaitForEndAnimation(string stateName, Action OnAnimationComplete = null, int layer = 0);
        public void SetIdleAnimation();
        public void SetMoveAnimation();
        public void SetAttackAnimation();
        public void SetGetHitAnimation();
        public void SetDieAnimation();

    }

    public class DefaultAnimationController : IAnimationController
    {
        public bool IsWaitingForEndAnimation => false;

        public void SetIdleAnimation()
        {
        }

        public void SetMoveAnimation()
        {
        }

        public void SetAttackAnimation()
        {
        }

        public void SetGetHitAnimation()
        {
        }

        public void SetDieAnimation()
        {
        }

        public void WaitForEndAnimation(string stateName, Action OnAnimationComplete = null, int layer = 0)
        { 
        }
    }
}
