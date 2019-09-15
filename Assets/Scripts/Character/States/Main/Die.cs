using AlirezaTarahomi.FightingGame.Character.Context;
using AlirezaTarahomi.FightingGame.Character.Event;
using Assets.Infrastructure.Scripts.CQRS;
using Infrastructure.StateMachine;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.State.Main
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
            _messageBus.RaiseEvent(new OnCharacterDied(context.CharacterId, context.PlayerId));
            context.AnimatorController.Die();
        }

        public override void Update(float deltaTime, StateMachine stateMachine, CharacterMainStateMachineContext context)
        {

        }

        public override void FixedUpdate(float deltaTime, StateMachine stateMachine, CharacterMainStateMachineContext context)
        {

        }

        public override void LateUpdate(float deltaTime, StateMachine stateMachine, CharacterMainStateMachineContext context)
        {

        }

        public override void Exit(CharacterMainStateMachineContext context)
        {

        }
    }
}