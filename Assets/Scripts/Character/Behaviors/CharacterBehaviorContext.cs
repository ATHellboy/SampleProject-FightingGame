using AlirezaTarahomi.FightingGame.Character.Event;
using AlirezaTarahomi.FightingGame.Service;
using UniRx;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Behavior
{
    public class CharacterBehaviorContext
    {
        public int jumpCounter;
        public bool isPowerupActive;
        public Collider2D hitboxCollider;

        public OnAttackEnded OnAttackEnded { get; private set; } = new();
        public OnFlyingToggled OnFlyingToggled { get; private set; } = new();
        public Transform Transform { get; private set; }
        public CharacterPivot Pivot { get; private set; }
        public IOwnershipService OwnershipService { get; private set; }
        public CharacterAnimatorController AnimatorController { get; private set; }
        public CharacterLocomotionHandler LocomotionHandler { get; private set; }
        public SurfaceCheck SurfaceCheck { get; private set; }
        public CompositeDisposable Disposables { get; private set; } = new();

        public CharacterBehaviorContext(Transform transform, CharacterPivot pivot, IOwnershipService ownershipService, 
            CharacterAnimatorController animatorController, CharacterLocomotionHandler locomotionHandler, 
            SurfaceCheck surfaceCheck)
        {
            Transform = transform;
            Pivot = pivot;
            OwnershipService = ownershipService;
            AnimatorController = animatorController;
            LocomotionHandler = locomotionHandler;
            SurfaceCheck = surfaceCheck;
        }
    }
}