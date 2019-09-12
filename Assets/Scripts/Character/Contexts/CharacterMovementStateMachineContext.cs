using UnityEngine;
using Zenject;
using AlirezaTarahomi.FightingGame.Character.State.Movement;

namespace AlirezaTarahomi.FightingGame.Character.Context
{
    public class CharacterMainStateMachineContext : BaseStateMachineContext
    {
        public CharacterLocomotionHandler LocomotionHandler { get; private set; }
        public CharacterAnimatorController AnimatorController { get; private set; }
        public int Id { get; private set; }
        public CharacterStats Stats { get; private set; }

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

        public Vector2 moveAxes;
        public bool goToNextState;
        public bool isInjured;

        public CharacterMainStateMachineContext(GameObject go, IState startState,
            [Inject(Id = "id")] int id, [Inject(Id = "stats")] CharacterStats stats,
            CharacterLocomotionHandler locomotionHandler, CharacterAnimatorController animatorController,
            Idle idle, Walk walk, Die die) : base(go, startState)
        {
            Id = id;
            Stats = stats;
            LocomotionHandler = locomotionHandler;
            AnimatorController = animatorController;
            RelatedStates = new States(idle, walk, die);
        }
    }
}