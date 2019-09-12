using AlirezaTarahomi.FightingGame.Character.Context;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.State.SecondaryMovement
{
    public class Land : BaseState<CharacterSecondaryMovementStateMachineContext>
    {
        public override void Enter(CharacterSecondaryMovementStateMachineContext context)
        {
            context.LocomotionHandler.SetGroundGravityScale();
        }

        public override void Update(StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {
            NextState = context.RelatedStates.none;
            stateMachine.ChangeState(this, NextState, context);
        }

        public override void FixedUpdate(StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {

        }

        public override void Exit(CharacterSecondaryMovementStateMachineContext context)
        {

        }
    }
}