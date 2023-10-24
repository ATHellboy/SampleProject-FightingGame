using System.Collections;
using UniRx;
using UnityEngine;
using VContainer;

namespace AlirezaTarahomi.FightingGame.Character.Behavior.Powerup
{
    [CreateAssetMenu(menuName = "Attacks/Powerup Attacks/FlyOverAttackBehavior")]
    public class FlyOverAttackBehavior : ScriptableObject, IPowerupAttackBehavior
    {
        [SerializeField] private float _velocity = 50;

        private CharacterBehaviorContext _context;

        [Inject]
        public void Construct(CharacterBehaviorContext context)
        {
            _context = context;
        }

        public Status BehaviorCondition
        {
            get
            {
                if (_context.isPowerupActive)
                {
                    return Status.Success;
                }
                return Status.Fail;
            }
        }

        public void Behave()
        {
            _context.OnFlyingToggled?.Invoke(true);
            _context.hitboxCollider.enabled = true;
            Observable.FromCoroutine(_ => FlyOver()).Subscribe();
        }

        public void EndBehavior()
        {
            _context.OnFlyingToggled?.Invoke(false);
            _context.hitboxCollider.enabled = false;
            _context.AnimatorController.ToggleAttacking(false);
        }

        IEnumerator FlyOver()
        {
            _context.LocomotionHandler.PushForward(_velocity);
            _context.AnimatorController.ToggleAttacking(true);

            while (_context.LocomotionHandler.GetVelocity().x != 0)
            {
                yield return null;
            }

            _context.OnFlyOverEnded?.Invoke();
            _context.OnAttackEnded?.Invoke();
        }
    }
}