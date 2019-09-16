using AlirezaTarahomi.FightingGame.Character.Context;
using Infrastructure.StateMachine;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.State.SecondaryMovement
{
    public class Jump : BaseState<CharacterSecondaryMovementStateMachineContext>
    {
        public override void Enter(CharacterSecondaryMovementStateMachineContext context)
        {
            context.LocomotionHandler.ChangeMoveSpeed(context.Stats.airMovementValues.inAirMoveSpeed);
            ExecuteJumping(context.jumpHeight, context.Stats.airMovementValues.jumpSpeed, context);
            context.AnimatorController.ToggleJumping(true);
        }

        public override void Update(float deltaTime, StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {
            if (context.isFlying)
            {
                stateMachine.ChangeState(this, context.RelatedStates.fly, context);
            }

            if (context.CheckNextJumpCondition())
            {
                context.jumpHeight = context.Stats.airMovementValues.lessJumpHeight;
                ExecuteJumping(context.jumpHeight, context.Stats.airMovementValues.jumpSpeed, context);
            }
        }

        public override void FixedUpdate(float deltaTime, StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {
            if (CheckFallCondition(context))
            {
                stateMachine.ChangeState(this, context.RelatedStates.fall, context);
            }
        }

        public override void LateUpdate(float deltaTime, StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {

        }

        public override void Exit(CharacterSecondaryMovementStateMachineContext context)
        {
            context.AnimatorController.ToggleJumping(false);
        }

        private void ExecuteJumping(float height, float speed, CharacterSecondaryMovementStateMachineContext context)
        {
            context.jumpCounter++;
            context.LocomotionHandler.Jump(height, speed);
        }

        private bool CheckFallCondition(CharacterSecondaryMovementStateMachineContext context)
        {
            if (context.LocomotionHandler.GetVelocity().y == 0 ||
                Mathf.Sign(context.LocomotionHandler.GetVelocity().y) == -1)
            {
                return true;
            }
            return false;
        }
    }
}