using AlirezaTarahomi.FightingGame.Character.Context;
using Infrastructure.StateMachine;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.State.SecondaryMovement
{
    public class Fly : BaseState<CharacterSecondaryMovementStateMachineContext>
    {
        public override void Enter(CharacterSecondaryMovementStateMachineContext context)
        {
            context.LocomotionHandler.SetNoGravityScale();
        }

        public override void Update(float deltaTime, StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {
            if (!context.isFlying)
            {
                stateMachine.ChangeState(this, context.RelatedStates.none, context);
            }
        }

        public override void FixedUpdate(float deltaTime, StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {

        }

        public override void LateUpdate(float deltaTime, StateMachine stateMachine, CharacterSecondaryMovementStateMachineContext context)
        {

        }

        public override void Exit(CharacterSecondaryMovementStateMachineContext context)
        {
            context.LocomotionHandler.Stop();
            context.LocomotionHandler.SetAirGravityScale();
        }
    }
}