using AlirezaTarahomi.FightingGame.Character.Context;
using Infrastructure.StateMachine;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.State.SecondaryMovement
{
    public class Fall : BaseState<CharacterSecondaryMovementStateMachineContext>
    {
        public override void Enter(CharacterSecondaryMovementStateMachineContext context)
        {
            context.LocomotionHandler.ChangeMoveSpeed(context.Stats.airMovementValues.inAirMoveSpeed);
            context.LocomotionHandler.SetAirGravityScale();
            context.AnimatorController.ToggleFalling(true);
        }

        public override void Update(float deltaTime, StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {
            if (context.isFlying)
            {
                NextState = context.RelatedStates.fly;
                stateMachine.ChangeState(this, NextState, context);
            }

            if (context.CheckNextJumpCondition())
            {
                context.jumpHeight = context.Stats.airMovementValues.lessJumpHeight;
                NextState = context.RelatedStates.jump;
                stateMachine.ChangeState(this, NextState, context);
            }

            if (context.jumpCounter > 1)
            {
                context.LocomotionHandler.ChangeDetectionCollisionMode(CollisionDetectionMode2D.Continuous);
            }
        }

        public override void FixedUpdate(float deltaTime, StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {
            if (context.isGrounded)
            {
                NextState = context.RelatedStates.land;
                stateMachine.ChangeState(this, NextState, context);
            }
        }

        public override void LateUpdate(float deltaTime, StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {

        }

        public override void Exit(CharacterSecondaryMovementStateMachineContext context)
        {
            context.AnimatorController.ToggleFalling(false);
            if (context.jumpCounter > 1)
            {
                context.LocomotionHandler.ChangeDetectionCollisionMode(CollisionDetectionMode2D.Discrete);
            }
        }
    }
}