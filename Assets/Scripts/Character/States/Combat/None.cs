using System.Collections;
using UnityEngine;
using UniRx;
using AlirezaTarahomi.FightingGame.Character.Powerup;
using AlirezaTarahomi.FightingGame.Character.Context;
using AlirezaTarahomi.FightingGame.InputSystem;

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

        public override void Update(StateMachine stateMachine, CharacterCombatStateMachineContext context)
        {
            if (_canAttack && _inputManager.IsDown("Attack_P" + context.Id))
            {
                NextState = context.RelatedStates.attack;
                stateMachine.ChangeState(this, NextState, context);
            }

            if (_inputManager.IsDown("PowerupAttack_P" + context.Id) && !context.isPowerupActive)
            {
                if (context.powerup.Active() == PowerType.OneTime)
                {
                    NextState = context.RelatedStates.attack;
                    stateMachine.ChangeState(this, NextState, context);
                }
            }
        }

        public override void FixedUpdate(StateMachine stateMachine, CharacterCombatStateMachineContext context)
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
    }
}