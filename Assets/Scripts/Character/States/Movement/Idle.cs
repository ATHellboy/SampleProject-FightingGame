using AlirezaTarahomi.FightingGame.InputSystem;
using AlirezaTarahomi.FightingGame.Character.Context;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.State.Movement
{
    public class Idle : BaseState<CharacterMainStateMachineContext>
    {
        private InputManager _inputManager;

        public Idle(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

        public override void Enter(CharacterMainStateMachineContext context)
        {
            context.LocomotionHandler.ChangeMoveSpeed(0);
            context.AnimatorController.ToggleStopping(true);
        }

        public override void Update(StateMachine stateMachine, CharacterMainStateMachineContext context)
        {
            if (context.isInjured)
            {
                NextState = context.RelatedStates.die;
                stateMachine.ChangeState(this, NextState, context);
            }

            if (context.moveAxes != Vector2.zero)
            {
                NextState = context.RelatedStates.walk;
                stateMachine.ChangeState(this, NextState, context);
            }
        }

        public override void FixedUpdate(StateMachine stateMachine, CharacterMainStateMachineContext context)
        {

        }

        public override void Exit(CharacterMainStateMachineContext context)
        {
            context.AnimatorController.ToggleStopping(false);
        }
    }
}