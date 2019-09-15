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
        }

        public override void Update(float deltaTime, StateMachine stateMachine, CharacterMainStateMachineContext context)
        {
            if (context.isInjured)
            {
                NextState = context.RelatedStates.die;
                stateMachine.ChangeState(this, NextState, context);
            }

            if (!context.CanControl)
                return;

            if (context.MoveAxes != Vector2.zero)
            {
                NextState = context.RelatedStates.walk;
                stateMachine.ChangeState(this, NextState, context);
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
        }

        public void ChangeMoveSpeed(CharacterMainStateMachineContext context)
        {
            context.LocomotionHandler.ChangeMoveSpeed(0);
        }
    }
}