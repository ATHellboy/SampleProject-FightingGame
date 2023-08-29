using AlirezaTarahomi.FightingGame.Character.Event;
using AlirezaTarahomi.FightingGame.Service;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Behavior
{
    public class CharacterBehaviorContext
    {
        public bool isGrounded;
        public int jumpCounter;
        public bool isPowerupActive;
        public Collider2D hitboxCollider;

        public OnAttackEnded OnAttackEnded { get; private set; } = new();
        public OnFlyingToggled OnFlyingToggled { get; private set; } = new();
        public OnFlyOverEnded OnFlyOverEnded { get; private set; } = new();
        public Transform Transform { get; private set; }
        public IOwnershipService OwnershipService { get; private set; }
        public CharacterAnimatorController AnimatorController { get; private set; }
        public CharacterLocomotionHandler LocomotionHandler { get; private set; }

        public CharacterBehaviorContext(Transform transform, IOwnershipService ownershipService, 
            CharacterAnimatorController animatorController, CharacterLocomotionHandler locomotionHandler)
        {
            Transform = transform;
            OwnershipService = ownershipService;
            AnimatorController = animatorController;
            LocomotionHandler = locomotionHandler;
        }
    }
}