using AlirezaTarahomi.FightingGame.Character.Behavior;
using AlirezaTarahomi.FightingGame.Character.Behavior.Complex;
using AlirezaTarahomi.FightingGame.Character.Behavior.Normal;
using AlirezaTarahomi.FightingGame.Character.Behavior.Powerup;
using AlirezaTarahomi.FightingGame.Character.Context;
using AlirezaTarahomi.FightingGame.Character.Event;
using Assets.Infrastructure.Scripts.CQRS;
using System.Collections.Generic;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.State.Combat
{
    public class Attack : BaseState<CharacterCombatStateMachineContext>
    {
        private IAttackBehavior _currentBehavior;
        private IMessageBus _messageBus;

        public Attack(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public override void Enter(CharacterCombatStateMachineContext context)
        {
            _messageBus.RaiseEvent(new OnAttackToggled(context.Id, context.Stats, true));

            for (int i = 0; i < context.AttackBehaviors.Count; i++)
            {
                _currentBehavior = context.AttackBehaviors[i];
                if (_currentBehavior != null && _currentBehavior.Behave() == Status.Success)
                {
                    break;
                }
            }
        }

        public override void Update(StateMachine stateMachine, CharacterCombatStateMachineContext context)
        {
            if (context.goToNextState)
            {
                context.goToNextState = false;

                NextState = context.RelatedStates.none;
                stateMachine.ChangeState(this, NextState, context);
            }
        }

        public override void FixedUpdate(StateMachine stateMachine, CharacterCombatStateMachineContext context)
        {

        }

        public override void Exit(CharacterCombatStateMachineContext context)
        {
            _currentBehavior.EndBehavior();
        }
    }
}