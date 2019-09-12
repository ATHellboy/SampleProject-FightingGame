using UnityEngine;
using Zenject;
using AlirezaTarahomi.FightingGame.Character.State.Movement;
using AlirezaTarahomi.FightingGame.Character.State.SecondaryMovement;

namespace AlirezaTarahomi.FightingGame.Character.Context
{
    public class CharacterSecondaryMovementStateMachineContext : BaseStateMachineContext
    {
        public CharacterLocomotionHandler LocomotionHandler { get; private set; }
        public CharacterAnimatorController AnimatorController { get; private set; }
        public int Id { get; private set; }
        public CharacterStats Stats { get; private set; }

        public class States
        {
            public None none;
            public Jump jump;
            public Land land;
            public Fall fall;

            public States(None none, Jump jump, Fall fall, Land land, Die die)
            {
                this.none = none;
                this.jump = jump;
                this.fall = fall;
                this.land = land;
            }
        }
        public States RelatedStates { get; }

        public Vector2 moveAxes;
        public bool isGrounded;
        public bool goToNextState;
        public int jumpCounter;

        public CharacterSecondaryMovementStateMachineContext(GameObject go, IState startState,
            [Inject(Id = "id")] int id, [Inject(Id = "stats")] CharacterStats stats,
            CharacterLocomotionHandler locomotionHandler, CharacterAnimatorController animatorController,
            None none, Jump jump, Fall fall, Land land, Die die) : base(go, startState)
        {
            Id = id;
            Stats = stats;
            LocomotionHandler = locomotionHandler;
            AnimatorController = animatorController;
            RelatedStates = new States(none, jump, fall, land, die);
        }
    }
}