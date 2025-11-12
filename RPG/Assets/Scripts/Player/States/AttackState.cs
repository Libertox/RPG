

using System.Collections;
using UnityEngine;

namespace Player
{
    public class AttackState : BaseState
    {
        private readonly ICombatController _combatController;
        private readonly PlayerController _playerController;

        private float _time = 0f;

        public AttackState(IAnimationController animationController, PlayerController playerController, ICombatController combatController) 
            : base(animationController)
        {
            _combatController = combatController;
            _playerController = playerController;
        }

        public override void OnEnter()
        {
            _animationController.SetAttackAnimation();

            _combatController.Attack();

            _time = 0;
        }

        public override void Update()
        {
            _time += Time.deltaTime;

            if(_time > _animationController.GetCurrentAnimatorStateInfo().length)
            {
                _playerController._isAttacking = false;
            }
        }
    }
}
