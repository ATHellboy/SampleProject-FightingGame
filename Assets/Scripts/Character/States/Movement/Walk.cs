using AlirezaTarahomi.FightingGame.InputSystem;
using System.Collections;
using System.Collections.Generic;
using AlirezaTarahomi.FightingGame.Character.Context;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.State.Movement
{
    public class Walk : BaseState<CharacterMainStateMachineContext>
    {
        private InputManager _inputManager;

        public Walk(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

        public override void Enter(CharacterMainStateMachineContext context)
        {
            context.LocomotionHandler.ChangeMoveSpeed(context.Stats.groundMovementValues.walkSpeed);
            context.AnimatorController.ToggleWalking(true);
        }

        public override void Update(StateMachine stateMachine, CharacterMainStateMachineContext context)
        {
            if (context.isInjured)
            {
                NextState = context.RelatedStates.die;
                stateMachine.ChangeState(this, NextState, context);
            }

            if (context.moveAxes == Vector2.zero)
            {
                NextState = context.RelatedStates.idle;
                stateMachine.ChangeState(this, NextState, context);
            }
        }

        public override void FixedUpdate(StateMachine stateMachine, CharacterMainStateMachineContext context)
        {
            context.LocomotionHandler.Move(context.moveAxes);
        }

        public override void Exit(CharacterMainStateMachineContext context)
        {
            context.AnimatorController.ToggleWalking(false);
        }
    }
}