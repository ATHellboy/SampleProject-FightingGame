using UnityEngine;
using Zenject;
using AlirezaTarahomi.FightingGame.Character.State.Main;
using AlirezaTarahomi.FightingGame.InputSystem;
using Infrastructure.StateMachine;

namespace AlirezaTarahomi.FightingGame.Character.Context
{
    public class CharacterMainStateMachineContext : BaseStateMachineContext
    {
        public bool isInjured;

        public InputManager InputManager { get; private set; }
        public CharacterLocomotionHandler LocomotionHandler { get; private set; }
        public CharacterAnimatorController AnimatorController { get; private set; }
        public string CharacterId { get; private set; }
        public int PlayerId { get; private set; }
        public CharacterStats Stats { get; private set; }
        public Vector2 MoveAxes
        {
            get
            {
                return new Vector2(InputManager.GetAxis("MoveHorizontal_P" + PlayerId),
                    InputManager.GetAxis("MoveVertical_P" + PlayerId));
            }
        }

        public class States
        {
            public Idle idle;
            public Walk walk;
            public Die die;

            public States(Idle idle, Walk walk, Die die)
            {
                this.idle = idle;
                this.walk = walk;
                this.die = die;
            }
        }
        public States RelatedStates { get; }

        public CharacterMainStateMachineContext(InputManager inputManager, GameObject go, IState startState,
            [Inject(Id = "id")] string characterId, [Inject(Id = "playerId")] int playerId, [Inject(Id = "stats")] CharacterStats stats, [Inject(Id = "debug")] bool debug,
            CharacterLocomotionHandler locomotionHandler, CharacterAnimatorController animatorController,
            Idle idle, Walk walk, Die die) : base(go, startState, debug)
        {
            InputManager = inputManager;
            CharacterId = characterId;
            PlayerId = playerId;
            Stats = stats;
            LocomotionHandler = locomotionHandler;
            AnimatorController = animatorController;
            RelatedStates = new States(idle, walk, die);
        }
    }
}