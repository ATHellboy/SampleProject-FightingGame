using UnityEngine;
using AlirezaTarahomi.FightingGame.Character.State.Main;
using Infrastructure.StateMachine;
using AlirezaTarahomi.FightingGame.Character.Event;

namespace AlirezaTarahomi.FightingGame.Character.Context
{
    public class CharacterMainStateMachineContext : BaseStateMachineContext
    {
        public Vector2 moveAxesRaw;
        public Vector2 moveAxes;
        public bool isInjured;

        public OnDied OnDied { get; private set; } = new();
        public CharacterLocomotionHandler LocomotionHandler { get; private set; }
        public CharacterAnimatorController AnimatorController { get; private set; }
        public CharacterContext CharacterContext { get; private set; }
        public SurfaceCheck SurfaceCheck { get; private set; }

        public class States
        {
            public Idle idle;
            public Move move;
            public Die die;

            public States(Idle idle, Move move, Die die)
            {
                this.idle = idle;
                this.move = move;
                this.die = die;
            }
        }
        public States RelatedStates { get; }

        public CharacterMainStateMachineContext(Transform transform, CharacterContext characterContext, 
            CharacterLocomotionHandler locomotionHandler, CharacterAnimatorController animatorController, SurfaceCheck surfaceCheck,
            Idle idle, Move move, Die die) : base(transform.gameObject, idle, characterContext.debugStateMachine)
        {
            CharacterContext = characterContext;
            LocomotionHandler = locomotionHandler;
            AnimatorController = animatorController;
            SurfaceCheck = surfaceCheck;
            RelatedStates = new States(idle, move, die);
        }
    }
}