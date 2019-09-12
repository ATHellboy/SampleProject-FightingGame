using AlirezaTarahomi.FightingGame.InputSystem;
using System.Collections;
using System.Collections.Generic;
using AlirezaTarahomi.FightingGame.Character.Context;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.State.SecondaryMovement
{
    public class Jump : BaseState<CharacterSecondaryMovementStateMachineContext>
    {
        private InputManager _inputManager;

        public Jump(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

        public override void Enter(CharacterSecondaryMovementStateMachineContext context)
        {
            context.jumpCounter = 1;
            context.LocomotionHandler.ChangeMoveSpeed(context.Stats.airMovementValues.inAirMoveSpeed);
            context.LocomotionHandler.Jump(context.Stats.airMovementValues.jumpHeight, context.Stats.airMovementValues.jumpSpeed);
            context.AnimatorController.ToggleJumping(true);
        }

        public override void Update(StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {
            if (context.jumpCounter < context.Stats.airMovementValues.jumpNumber && _inputManager.IsDown("Jump_P" + context.Id))
            {
                context.jumpCounter++;
                context.LocomotionHandler.Jump(context.Stats.airMovementValues.lessJumpHeight, context.Stats.airMovementValues.jumpSpeed);
            }
        }

        public override void FixedUpdate(StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {
            if (context.LocomotionHandler.GetVelocity().y == 0 || Mathf.Sign(context.LocomotionHandler.GetVelocity().y) == -1)
            {
                NextState = context.RelatedStates.fall;
                stateMachine.ChangeState(this, NextState, context);
            }
        }

        public override void Exit(CharacterSecondaryMovementStateMachineContext context)
        {
            context.AnimatorController.ToggleJumping(false);
        }
    }
}