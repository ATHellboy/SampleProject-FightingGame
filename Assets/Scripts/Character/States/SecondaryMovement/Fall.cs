using AlirezaTarahomi.FightingGame.InputSystem;
using System.Collections;
using System.Collections.Generic;
using AlirezaTarahomi.FightingGame.Character.Context;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.State.SecondaryMovement
{
    public class Fall : BaseState<CharacterSecondaryMovementStateMachineContext>
    {
        private InputManager _inputManager;

        public Fall(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

        public override void Enter(CharacterSecondaryMovementStateMachineContext context)
        {
            context.LocomotionHandler.ChangeMoveSpeed(context.Stats.airMovementValues.inAirMoveSpeed);
            context.LocomotionHandler.SetAirGravityScale();
            context.AnimatorController.ToggleJumping(true);
        }

        public override void Update(StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {
            if (context.jumpCounter < context.Stats.airMovementValues.jumpNumber && _inputManager.IsDown("Jump_P" + context.Id))
            {
                context.jumpCounter++;
                context.LocomotionHandler.Jump(context.Stats.airMovementValues.lessJumpHeight, context.Stats.airMovementValues.jumpSpeed);
            }

            if (context.jumpCounter > 1)
            {
                context.LocomotionHandler.ChangeDetectionCollisionMode(CollisionDetectionMode2D.Continuous);
            }
        }

        public override void FixedUpdate(StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {
            if (context.isGrounded)
            {
                NextState = context.RelatedStates.land;
                stateMachine.ChangeState(this, NextState, context);
            }
        }

        public override void Exit(CharacterSecondaryMovementStateMachineContext context)
        {
            context.AnimatorController.ToggleJumping(false);
            if (context.jumpCounter > 1)
            {
                context.LocomotionHandler.ChangeDetectionCollisionMode(CollisionDetectionMode2D.Discrete);
            }
        }
    }
}