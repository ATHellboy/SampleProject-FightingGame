using System.Collections;
using UnityEngine;
using UniRx;
using AlirezaTarahomi.FightingGame.Character.Powerup;
using AlirezaTarahomi.FightingGame.Character.Context;
using AlirezaTarahomi.FightingGame.InputSystem;
using Assets.Infrastructure.Scripts.CQRS;
using AlirezaTarahomi.FightingGame.Character.Event;

namespace AlirezaTarahomi.FightingGame.Character.State.SecondaryMovement
{
    public class None : BaseState<CharacterSecondaryMovementStateMachineContext>
    {
        private InputManager _inputManager;
        private IMessageBus _messageBus;

        public None(InputManager inputManager, IMessageBus messageBus)
        {
            _inputManager = inputManager;
            _messageBus = messageBus;
        }

        public override void Enter(CharacterSecondaryMovementStateMachineContext context)
        {
            _messageBus.RaiseEvent(new OnSpeedChanged(context.Id, context.Stats));
        }

        public override void Update(StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {
            if (_inputManager.IsDown("Jump_P" + context.Id))
            {
                NextState = context.RelatedStates.jump;
                stateMachine.ChangeState(this, NextState, context);
            }
        }

        public override void FixedUpdate(StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {
            if (!context.isGrounded)
            {
                NextState = context.RelatedStates.fall;
                stateMachine.ChangeState(this, NextState, context);
            }
        }

        public override void Exit(CharacterSecondaryMovementStateMachineContext context)
        {

        }
    }
}