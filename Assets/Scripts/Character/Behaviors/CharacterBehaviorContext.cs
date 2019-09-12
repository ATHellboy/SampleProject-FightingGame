using AlirezaTarahomi.FightingGame.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Character.Behavior
{
    public class CharacterBehaviorContext
    {
        public CharacterStats Stats { get; private set; }
        public int PlayerId { get; private set; }
        public Transform Transform { get; private set; }
        public IOwnershipService OwnershipService { get; private set; }
        public CharacterAnimatorController AnimatorController { get; private set; }
        public CharacterLocomotionHandler LocomotionHandler { get; private set; }

        public bool isGrounded;
        public int jumpCounter;
        public bool isPowerupActive;
        public Collider2D hitboxCollider;

        public CharacterBehaviorContext([Inject(Id = "stats")] CharacterStats stats,
            [Inject(Id = "id")] int playerId, Transform transform, IOwnershipService ownershipService,
            CharacterAnimatorController animatorController, CharacterLocomotionHandler locomotionHandler)
        {
            Stats = stats;
            PlayerId = playerId;
            Transform = transform;
            OwnershipService = ownershipService;
            AnimatorController = animatorController;
            LocomotionHandler = locomotionHandler;
        }
    }
}