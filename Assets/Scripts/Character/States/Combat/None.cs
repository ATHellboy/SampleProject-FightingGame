using System.Collections;
using UnityEngine;
using UniRx;
using AlirezaTarahomi.FightingGame.Character.Powerup;
using AlirezaTarahomi.FightingGame.Character.Context;
using Infrastructure.StateMachine;

namespace AlirezaTarahomi.FightingGame.Character.State.Combat
{
    public class None : BaseState<CharacterCombatStateMachineContext>
    {
        private bool _canAttack;

        public override void Enter(CharacterCombatStateMachineContext context)
        {
            Observable.FromCoroutine(_ => AttackTimer(context)).Subscribe();
        }

        public override void Update(float deltaTime, StateMachine stateMachine, CharacterCombatStateMachineContext context)
        {
            if (!context.CanControl)
                return;

            if (CheckAttackCondition(context))
            {
                stateMachine.ChangeState(this, context.RelatedStates.attack, context);
            }

            if (CheckPowerupCondition(context))
            {
                if (context.powerup.Active() == PowerType.OneTime)
                {
                    stateMachine.ChangeState(this, context.RelatedStates.attack, context);
                }
            }
        }

        public override void FixedUpdate(float deltaTime, StateMachine stateMachine, CharacterCombatStateMachineContext context)
        {

        }

        public override void LateUpdate(float deltaTime, StateMachine stateMachine, CharacterCombatStateMachineContext context)
        {

        }

        public override void Exit(CharacterCombatStateMachineContext context)
        {
            _canAttack = false;
        }

        IEnumerator AttackTimer(CharacterCombatStateMachineContext context)
        {
            yield return new WaitForSeconds(context.Stats.miscValues.attackRate);
            _canAttack = true;
        }

        private bool CheckAttackCondition(CharacterCombatStateMachineContext context)
        {
            if (context.isAttackedPressed && _canAttack)
            {
                return true;
            }
            return false;
        }

        private bool CheckPowerupCondition(CharacterCombatStateMachineContext context)
        {
            if (context.isPowerupAttackedPressed && !context.isPowerupActive)
            {
                return true;
            }
            return false;
        }
    }
}