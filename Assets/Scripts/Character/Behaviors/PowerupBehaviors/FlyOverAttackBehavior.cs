using AlirezaTarahomi.FightingGame.Character.Powerup;
using System.Collections;
using UniRx;
using UnityEngine;
using VContainer;

namespace AlirezaTarahomi.FightingGame.Character.Behavior.Powerup
{
    [CreateAssetMenu(menuName = "Custom/Attacks/Powerup Attacks/FlyOverAttackBehavior")]
    public class FlyOverAttackBehavior : ScriptableObject, IPowerupAttackBehavior
    {
        [SerializeField] private float _velocity = 50;

        private CharacterBehaviorContext _behaviorContext;
        private CharacterPowerupContext _powerupContext;
        private CompositeDisposable _disposables = new();

        [Inject]
        public void Construct(CharacterBehaviorContext behaviorContext, CharacterPowerupContext powerupContext)
        {
            _behaviorContext = behaviorContext;
            _powerupContext = powerupContext;
        }

        void OnDisable()
        {
            _disposables.Dispose();
        }

        public Status BehaviorCondition
        {
            get
            {
                if (_behaviorContext.isPowerupActive)
                {
                    return Status.Success;
                }
                return Status.Fail;
            }
        }

        public void Behave()
        {
            _behaviorContext.OnFlyingToggled?.Invoke(true);
            _behaviorContext.hitboxCollider.enabled = true;
            Observable.FromCoroutine(_ => FlyOver()).Subscribe().AddTo(_behaviorContext.Disposables);
        }

        public void EndBehavior()
        {
            _behaviorContext.OnFlyingToggled?.Invoke(false);
            _behaviorContext.hitboxCollider.enabled = false;
            _behaviorContext.AnimatorController.ToggleAttacking(false);
        }

        IEnumerator FlyOver()
        {
            _behaviorContext.LocomotionHandler.PushForward(_velocity);
            _behaviorContext.AnimatorController.ToggleAttacking(true);

            while (_behaviorContext.LocomotionHandler.GetVelocity().x != 0)
            {
                yield return null;
            }

            _powerupContext.OnPowerupEnded?.Invoke();
            _behaviorContext.OnAttackEnded?.Invoke();
        }
    }
}