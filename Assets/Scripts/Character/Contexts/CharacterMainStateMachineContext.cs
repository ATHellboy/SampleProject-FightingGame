using UnityEngine;
using AlirezaTarahomi.FightingGame.Character.State.Main;
using Infrastructure.StateMachine;
using AlirezaTarahomi.FightingGame.Character.Event;

namespace AlirezaTarahomi.FightingGame.Character.Context
{
    public class CharacterMainStateMachineContext : BaseStateMachineContext
    {
        public Vector2 moveAxes;
        public bool isInjured;

        public OnDied OnDied { get; private set; } = new();
        public CharacterLocomotionHandler LocomotionHandler { get; private set; }
        public CharacterAnimatorController AnimatorController { get; private set; }
        public CharacterContext CharacterContext { get; private set; }

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

        public CharacterMainStateMachineContext(Transform transform, CharacterContext characterContext, 
            CharacterLocomotionHandler locomotionHandler, CharacterAnimatorController animatorController, 
            Idle idle, Walk walk, Die die) : base(transform.gameObject, idle, characterContext.debugStateMachine)
        {
            CharacterContext = characterContext;
            LocomotionHandler = locomotionHandler;
            AnimatorController = animatorController;
            RelatedStates = new States(idle, walk, die);
        }
    }
}