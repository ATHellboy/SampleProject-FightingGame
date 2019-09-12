using UnityEngine;
using System;
using Zenject;
using AlirezaTarahomi.FightingGame.Character.State.Combat;
using System.Collections.Generic;
using AlirezaTarahomi.FightingGame.Character.Behavior;
using AlirezaTarahomi.FightingGame.Character.Powerup;

namespace AlirezaTarahomi.FightingGame.Character.Context
{
    public class CharacterCombatStateMachineContext : BaseStateMachineContext
    {
        public IPowerup powerup;

        public int Id { get; private set; }
        public CharacterStats Stats { get; private set; }
        public CharacterAnimatorController AnimatorController { get; private set; }
        public List<IAttackBehavior> AttackBehaviors { get; private set; } = new List<IAttackBehavior>();

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

        public bool isGrounded;
        public bool goToNextState;
        public bool isPowerupActive;

        public CharacterCombatStateMachineContext(GameObject go, IState startState,
            [Inject(Id = "id")] int id, [Inject(Id = "stats")] CharacterStats stats,
            CharacterAnimatorController animatorController, None none, Attack attack) : base(go, startState)
        {
            Id = id;
            Stats = stats;
            AnimatorController = animatorController;
            RelatedStates = new States(none, attack);
        }

        public void AddAttackBehavior(IAttackBehavior attackBehavior)
        {
            AttackBehaviors.Add(attackBehavior);
        }
    }
}