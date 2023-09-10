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
        private bool _canAttack = true;
        private bool _isPowerupInCooldown = false;

        public override void Enter(CharacterCombatStateMachineContext context)
        {
            
        }

        public override void Update(float deltaTime, StateMachine stateMachine, CharacterCombatStateMachineContext context)
        {
            if (!context.CanControl)
                return;

            if (CheckAttackCondition(context))
            {
                _canAttack = false;
                Observable.FromCoroutine(_ => AttackTimer(context)).Subscribe();
                stateMachine.ChangeState(this, context.RelatedStates.attack, context);
            }

            if (CheckPowerupCondition(context))
            {
                PowerupType powerupType = context.powerup.Active();
                _isPowerupInCooldown = true;
                Observable.FromCoroutine(_ => PowerupCooldownTimer(context)).Subscribe();
                if (powerupType == PowerupType.OneTime)
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
            
        }

        IEnumerator AttackTimer(CharacterCombatStateMachineContext context)
        {
            yield return new WaitForSeconds(context.Stats.miscValues.attackRate);
            _canAttack = true;
        }

        IEnumerator PowerupCooldownTimer(CharacterCombatStateMachineContext context)
        {
            yield return new WaitForSeconds((context.Stats.behaviors.powerup.value as IPowerup).PowerupCooldown);
            _isPowerupInCooldown = false;
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
            if (context.isPowerupAttackedPressed && !context.isPowerupActive && !_isPowerupInCooldown)
            {
                return true;
            }
            return false;
        }
    }
}