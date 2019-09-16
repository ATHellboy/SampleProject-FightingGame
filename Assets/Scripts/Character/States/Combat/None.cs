using System.Collections;
using UnityEngine;
using UniRx;
using AlirezaTarahomi.FightingGame.Character.Powerup;
using AlirezaTarahomi.FightingGame.Character.Context;
using AlirezaTarahomi.FightingGame.InputSystem;
using Infrastructure.StateMachine;

namespace AlirezaTarahomi.FightingGame.Character.State.Combat
{
    public class None : BaseState<CharacterCombatStateMachineContext>
    {
        private InputManager _inputManager;
        private bool _canAttack;

        public None(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

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
            if (_inputManager.IsDown("Attack_P" + context.PlayerId) && _canAttack)
            {
                return true;
            }
            return false;
        }

        private bool CheckPowerupCondition(CharacterCombatStateMachineContext context)
        {
            if (_inputManager.IsDown("PowerupAttack_P" + context.PlayerId) && !context.isPowerupActive)
            {
                return true;
            }
            return false;
        }
    }
}