using AlirezaTarahomi.FightingGame.Character.Context;
using Infrastructure.StateMachine;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.State.Main
{
    public class Idle : BaseState<CharacterMainStateMachineContext>, IMainState
    {
        public override void Enter(CharacterMainStateMachineContext context)
        {
            ChangeMoveSpeed(context);
            context.AnimatorController.ToggleStopping(true);
            if (context.SurfaceCheck.OnSlope)
            {
                context.LocomotionHandler.Stop();
                context.LocomotionHandler.SetNoGravityScale();
            }
        }

        public override void Update(float deltaTime, StateMachine stateMachine, CharacterMainStateMachineContext context)
        {
            context.LocomotionHandler.StopSliding();
            if (context.isInjured)
            {
                stateMachine.ChangeState(this, context.RelatedStates.die, context);
            }
            
            if (!context.CanControl)
                return;

            if (context.moveAxesRaw.x != 0)
            {
                stateMachine.ChangeState(this, context.RelatedStates.move, context);
            }
        }

        public override void FixedUpdate(float deltaTime, StateMachine stateMachine, CharacterMainStateMachineContext context)
        {
            
        }

        public override void LateUpdate(float deltaTime, StateMachine stateMachine, CharacterMainStateMachineContext context)
        {

        }

        public override void Exit(CharacterMainStateMachineContext context)
        {
            context.AnimatorController.ToggleStopping(false);
            if (context.SurfaceCheck.onGround)
            {
                context.LocomotionHandler.SetGroundGravityScale();
            }
        }

        public void ChangeMoveSpeed(CharacterMainStateMachineContext context)
        {
            context.LocomotionHandler.ChangeMoveSpeed(0);
        }
    }
}