using UnityEngine;
using AlirezaTarahomi.FightingGame.Character.State.Combat;
using System.Collections.Generic;
using AlirezaTarahomi.FightingGame.Character.Behavior;
using AlirezaTarahomi.FightingGame.Character.Powerup;
using Infrastructure.StateMachine;
using AlirezaTarahomi.FightingGame.Character.Event;

namespace AlirezaTarahomi.FightingGame.Character.Context
{
    public class CharacterCombatStateMachineContext : BaseStateMachineContext
    {
        public bool isAttackedPressed;
        public bool isPowerupAttackedPressed;
        public IPowerup powerup;
        public bool isGrounded;
        public bool isAttackingEnded;
        public bool isPowerupActive;

        public OnAttackStarted OnAttackStarted { get; private set; } = new();
        public CharacterContext CharacterContext { get; private set; }
        public List<IAttackBehavior> AttackBehaviors { get; private set; } = new();

        public class States
        {
            public None none;
            public Attack attack;

            public States(None none, Attack attack)
            {
                this.none = none;
                this.attack = attack;
            }
        }
        public States RelatedStates { get; }

        public CharacterCombatStateMachineContext(Transform transform, CharacterContext characterContext, 
            None none, Attack attack) : base(transform.gameObject, none, characterContext.debugStateMachine)
        {
            CharacterContext = characterContext;
            RelatedStates = new States(none, attack);
        }

        public void AddAttackBehavior(IAttackBehavior attackBehavior)
        {
            AttackBehaviors.Add(attackBehavior);
        }
    }
}