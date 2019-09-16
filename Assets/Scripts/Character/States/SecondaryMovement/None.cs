using UnityEngine;
using AlirezaTarahomi.FightingGame.Character.Context;
using Assets.Infrastructure.Scripts.CQRS;
using AlirezaTarahomi.FightingGame.Character.Event;
using Infrastructure.StateMachine;

namespace AlirezaTarahomi.FightingGame.Character.State.SecondaryMovement
{
    public class None : BaseState<CharacterSecondaryMovementStateMachineContext>
    {
        private IMessageBus _messageBus;

        public None(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public override void Enter(CharacterSecondaryMovementStateMachineContext context)
        {
            _messageBus.RaiseEvent(new OnSecondaryMovementNoneStateEntered(context.CharacterId));
        }

        public override void Update(float deltaTime, StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {
            if (context.isFlying)
            {
                stateMachine.ChangeState(this, context.RelatedStates.fly, context);
            }

            if (!context.CanControl)
                return;

            if (context.InputManager.IsDown("Jump_P" + context.PlayerId))
            {
                context.jumpHeight = context.Stats.airMovementValues.jumpHeight;
                stateMachine.ChangeState(this, context.RelatedStates.jump, context);
            }
        }

        public override void FixedUpdate(float deltaTime, StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {
            if (!context.isGrounded)
            {
                stateMachine.ChangeState(this, context.RelatedStates.fall, context);
            }
        }

        public override void LateUpdate(float deltaTime, StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {

        }

        public override void Exit(CharacterSecondaryMovementStateMachineContext context)
        {

        }
    }
}