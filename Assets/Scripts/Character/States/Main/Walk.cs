using AlirezaTarahomi.FightingGame.Character.Context;
using Infrastructure.StateMachine;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.State.Main
{
    public class Walk : BaseState<CharacterMainStateMachineContext>, IMainState
    {
        public override void Enter(CharacterMainStateMachineContext context)
        {
            ChangeMoveSpeed(context);
            context.AnimatorController.ToggleWalking(true);
        }

        public override void Update(float deltaTime, StateMachine stateMachine, CharacterMainStateMachineContext context)
        {
            if (context.isInjured)
            {
                stateMachine.ChangeState(this, context.RelatedStates.die, context);
            }

            if (context.MoveAxes == Vector2.zero)
            {
                stateMachine.ChangeState(this, context.RelatedStates.idle, context);
            }
        }

        public override void FixedUpdate(float deltaTime, StateMachine stateMachine, CharacterMainStateMachineContext context)
        {
            if (!context.CanControl)
                return;

            context.LocomotionHandler.Move(context.MoveAxes);
        }

        public override void LateUpdate(float deltaTime, StateMachine stateMachine, CharacterMainStateMachineContext context)
        {

        }

        public override void Exit(CharacterMainStateMachineContext context)
        {
            context.AnimatorController.ToggleWalking(false);
        }

        public void ChangeMoveSpeed(CharacterMainStateMachineContext context)
        {
            context.LocomotionHandler.ChangeMoveSpeed(context.Stats.groundMovementValues.walkSpeed);
        }
    }
}