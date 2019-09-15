using UnityEngine;
using System;
using Zenject;
using AlirezaTarahomi.FightingGame.Character.State.Combat;
using System.Collections.Generic;
using AlirezaTarahomi.FightingGame.Character.Behavior;
using AlirezaTarahomi.FightingGame.Character.Powerup;
using Infrastructure.StateMachine;

namespace AlirezaTarahomi.FightingGame.Character.Context
{
    public class CharacterCombatStateMachineContext : BaseStateMachineContext
    {
        public IPowerup powerup;
        public bool isGrounded;
        public bool isAttackingEnded;
        public bool isPowerupActive;

        public string CharacterId { get; private set; }
        public int PlayerId { get; private set; }
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

        public CharacterCombatStateMachineContext(GameObject go, IState startState, [Inject(Id = "id")] string characterId,
            [Inject(Id = "playerId")] int playerId, [Inject(Id = "stats")] CharacterStats stats, [Inject(Id = "debug")] bool debug,
            CharacterAnimatorController animatorController, None none, Attack attack) : base(go, startState, debug)
        {
            CharacterId = characterId;
            PlayerId = playerId;
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