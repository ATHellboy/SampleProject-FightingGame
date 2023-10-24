using UnityEngine;
using AlirezaTarahomi.FightingGame.Character.Context;
using Infrastructure.StateMachine;

namespace AlirezaTarahomi.FightingGame.Character.State.SecondaryMovement
{
    public class None : BaseState<CharacterSecondaryMovementStateMachineContext>
    {
        public override void Enter(CharacterSecondaryMovementStateMachineContext context)
        {
            context.OnChangeMoveSpeedRequested?.Invoke();
        }

        public override void Update(float deltaTime, StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {
            if (context.isFlying)
            {
                stateMachine.ChangeState(this, context.RelatedStates.fly, context);
            }

            if (!context.CanControl)
                return;

            if (context.isJumpedPressed)
            {
                context.jumpHeight = context.CharacterContext.stats.airMovementValues.jumpHeight;
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