using System.Collections;
using UniRx;
using UnityEngine;
using VContainer;

namespace AlirezaTarahomi.FightingGame.Character.Behavior.Complex
{
    [CreateAssetMenu(menuName = "Attacks/Complex Attacks/ForwardMeleeAttackBehavior")]
    public class ForwardMeleeAttackBehavior : ScriptableObject, IComplexAttackBehavior
    {
        [SerializeField] private float _velocity = 50;
        [SerializeField] private float _distance = 8;

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
                if (!_context.isGrounded)
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
            _context.AnimatorController.ToggleAttacking(true);
            Observable.FromCoroutine(_ => MoveForwardAndAttack()).Subscribe().AddTo(_context.Disposables);
        }

        public void EndBehavior()
        {
            _context.OnFlyingToggled?.Invoke(false);
            _context.hitboxCollider.enabled = false;
            _context.AnimatorController.ToggleAttacking(false);
        }

        IEnumerator MoveForwardAndAttack()
        {
            _context.LocomotionHandler.PushForward(_velocity);

            yield return new WaitForSeconds(_distance / _velocity);

            _context.OnAttackEnded?.Invoke();
        }
    }
}