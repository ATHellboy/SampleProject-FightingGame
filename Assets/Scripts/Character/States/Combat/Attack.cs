using AlirezaTarahomi.FightingGame.Character.Behavior;
using AlirezaTarahomi.FightingGame.Character.Context;
using AlirezaTarahomi.FightingGame.Character.Event;
using Assets.Infrastructure.Scripts.CQRS;
using Infrastructure.StateMachine;
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
            _messageBus.RaiseEvent(new OnAttackToggled(context.CharacterId, context.PlayerId, true));
            ExecuteAttacking(context);
        }

        public override void Update(float deltaTime, StateMachine stateMachine, CharacterCombatStateMachineContext context)
        {
            if (context.isAttackingEnded || _currentBehavior == null)
            {
                context.isAttackingEnded = false;
                stateMachine.ChangeState(this, context.RelatedStates.none, context);
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
            _currentBehavior.EndBehavior();
            _currentBehavior = null;
        }

        private void ExecuteAttacking(CharacterCombatStateMachineContext context)
        {
            for (int i = 0; i < context.AttackBehaviors.Count; i++)
            {
                _currentBehavior = context.AttackBehaviors[i];
                if (_currentBehavior != null && _currentBehavior.BehaviorCondition == Status.Success)
                {
                    _currentBehavior.Behave();
                    break;
                }
            }
        }
    }
}