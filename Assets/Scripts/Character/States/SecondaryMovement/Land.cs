using AlirezaTarahomi.FightingGame.Character.Context;
using Infrastructure.StateMachine;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.State.SecondaryMovement
{
    public class Land : BaseState<CharacterSecondaryMovementStateMachineContext>
    {
        public override void Enter(CharacterSecondaryMovementStateMachineContext context)
        {
            context.LocomotionHandler.SetGroundGravityScale();
            context.AnimatorController.ToggleLanding(true);
        }

        public override void Update(float deltaTime, StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {
            stateMachine.ChangeState(this, context.RelatedStates.none, context);
        }

        public override void FixedUpdate(float deltaTime, StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {

        }

        public override void LateUpdate(float deltaTime, StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {

        }

        public override void Exit(CharacterSecondaryMovementStateMachineContext context)
        {
            context.jumpCounter = 0;
            context.AnimatorController.ToggleLanding(false);
        }
    }
}