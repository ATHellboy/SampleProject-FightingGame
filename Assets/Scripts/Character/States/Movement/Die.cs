using AlirezaTarahomi.FightingGame.Character.Context;
using AlirezaTarahomi.FightingGame.Character.Event;
using Assets.Infrastructure.Scripts.CQRS;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.State.Movement
{
    public class Die : BaseState<CharacterMainStateMachineContext>
    {
        private IMessageBus _messageBus;

        public Die(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public override void Enter(CharacterMainStateMachineContext context)
        {
            _messageBus.RaiseEvent(new OnCharacterDied(context.Id, context.Stats));
            context.AnimatorController.Die();
        }

        public override void Update(StateMachine stateMachine, CharacterMainStateMachineContext context)
        {

        }

        public override void FixedUpdate(StateMachine stateMachine, CharacterMainStateMachineContext context)
        {

        }

        public override void Exit(CharacterMainStateMachineContext context)
        {

        }
    }
}