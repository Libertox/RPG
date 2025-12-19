
using UnityEngine;

namespace Entity
{
    public class AttackState : BaseState
    {
        private readonly IAnimationController _animationController;
        private readonly ICombatController _combatController;

        public AttackState(EntityController entityController) 
            : base(entityController)
        {
            _animationController = entityController.AnimationController;
            _combatController = entityController.CombatController;
        }

        public override void OnEnter()
        {
            _animationController.SetAttackAnimation();

            _combatController.Attack();

            _entityController.StartCoroutine(_animationController.WaitForEndAnimation(AnimationName.ATTACK, OnAttackAnimationComplete));      
        }

        public void OnAttackAnimationComplete()
        {
            _combatController.SetIsAttacking(false);
        }

    }
}
