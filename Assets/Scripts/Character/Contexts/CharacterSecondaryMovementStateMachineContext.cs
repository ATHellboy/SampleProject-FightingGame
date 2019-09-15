using UnityEngine;
using Zenject;
using AlirezaTarahomi.FightingGame.Character.State.SecondaryMovement;
using AlirezaTarahomi.FightingGame.InputSystem;
using Infrastructure.StateMachine;

namespace AlirezaTarahomi.FightingGame.Character.Context
{
    public class CharacterSecondaryMovementStateMachineContext : BaseStateMachineContext
    {
        public Vector2 moveAxes;
        public bool isGrounded;
        public float jumpHeight;
        public int jumpCounter;
        public bool isFlying;

        public InputManager InputManager { get; private set; }
        public CharacterLocomotionHandler LocomotionHandler { get; private set; }
        public CharacterAnimatorController AnimatorController { get; private set; }
        public string CharacterId { get; private set; }
        public int PlayerId { get; private set; }
        public CharacterStats Stats { get; private set; }

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

        public CharacterSecondaryMovementStateMachineContext(InputManager inputManager, GameObject go, IState startState,
            [Inject(Id = "id")] string characterId, [Inject(Id = "playerId")] int playerId, [Inject(Id = "stats")] CharacterStats stats, [Inject(Id = "debug")] bool debug,
            CharacterLocomotionHandler locomotionHandler, CharacterAnimatorController animatorController,
            None none, Jump jump, Fall fall, Land land, Fly fly) : base(go, startState, debug)
        {
            InputManager = inputManager;
            CharacterId = characterId;
            PlayerId = playerId;
            Stats = stats;
            LocomotionHandler = locomotionHandler;
            AnimatorController = animatorController;
            RelatedStates = new States(none, jump, fall, land, fly);
        }

        public bool CheckNextJumpCondition()
        {
            if (!CanControl)
                return false;

            if (jumpCounter < Stats.airMovementValues.jumpNumber &&
                InputManager.IsDown("Jump_P" + PlayerId))
            {
                return true;
            }
            return false;
        }
    }
}