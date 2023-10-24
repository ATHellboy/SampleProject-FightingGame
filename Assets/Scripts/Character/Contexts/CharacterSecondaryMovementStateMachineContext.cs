using UnityEngine;
using AlirezaTarahomi.FightingGame.Character.State.SecondaryMovement;
using Infrastructure.StateMachine;
using AlirezaTarahomi.FightingGame.Character.Event;

namespace AlirezaTarahomi.FightingGame.Character.Context
{
    public class CharacterSecondaryMovementStateMachineContext : BaseStateMachineContext
    {
        public bool isJumpedPressed;
        public bool isGrounded;
        public float jumpHeight;
        public int jumpCounter;
        public bool isFlying;

        public OnChangeMoveSpeedRequested OnChangeMoveSpeedRequested { get; private set; } = new();
        public CharacterLocomotionHandler LocomotionHandler { get; private set; }
        public CharacterAnimatorController AnimatorController { get; private set; }
        public CharacterContext CharacterContext { get; private set; }

        public class States
        {
            public None none;
            public Jump jump;
            public Land land;
            public Fall fall;
            public Fly fly;

            public States(None none, Jump jump, Fall fall, Land land, Fly fly)
            {
                this.none = none;
                this.jump = jump;
                this.fall = fall;
                this.land = land;
                this.fly = fly;
            }
        }
        public States RelatedStates { get; }

        public CharacterSecondaryMovementStateMachineContext(Transform transform, CharacterContext characterContext, 
            CharacterLocomotionHandler locomotionHandler, CharacterAnimatorController animatorController, 
            None none, Jump jump, Fall fall, Land land, Fly fly) : base(transform.gameObject, none, characterContext.debugStateMachine)
        {
            CharacterContext = characterContext;
            LocomotionHandler = locomotionHandler;
            AnimatorController = animatorController;
            RelatedStates = new States(none, jump, fall, land, fly);
        }

        public bool CheckNextJumpCondition()
        {
            if (!CanControl)
                return false;

            if (jumpCounter < CharacterContext.stats.airMovementValues.jumpNumber && isJumpedPressed)
            {
                return true;
            }
            return false;
        }
    }
}