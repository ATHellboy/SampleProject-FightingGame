using AlirezaTarahomi.FightingGame.Character.Context;
using Infrastructure.StateMachine;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.State.SecondaryMovement
{
    public class Fall : BaseState<CharacterSecondaryMovementStateMachineContext>
    {
        public override void Enter(CharacterSecondaryMovementStateMachineContext context)
        {
            context.LocomotionHandler.ChangeMoveSpeed(context.CharacterContext.stats.airMovementValues.inAirMoveSpeed);
            context.LocomotionHandler.SetAirGravityScale();
            context.AnimatorController.ToggleFalling(true);
        }

        public override void Update(float deltaTime, StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {
            if (context.isFlying)
            {
                stateMachine.ChangeState(this, context.RelatedStates.fly, context);
            }

            if (context.CheckNextJumpCondition())
            {
                context.jumpHeight = context.CharacterContext.stats.airMovementValues.lessJumpHeight;
                stateMachine.ChangeState(this, context.RelatedStates.jump, context);
            }
        }

        public override void FixedUpdate(float deltaTime, StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {
            if (context.SurfaceCheck.onGround)
            {
                stateMachine.ChangeState(this, context.RelatedStates.land, context);
            }
        }

        public override void LateUpdate(float deltaTime, StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {

        }

        public override void Exit(CharacterSecondaryMovementStateMachineContext context)
        {
            context.AnimatorController.ToggleFalling(false);
        }
    }
}