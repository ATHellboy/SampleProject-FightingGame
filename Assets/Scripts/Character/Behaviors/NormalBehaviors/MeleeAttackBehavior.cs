using UnityEngine;
using System.Collections;
using UniRx;
using VContainer;

namespace AlirezaTarahomi.FightingGame.Character.Behavior.Normal
{
    [CreateAssetMenu(menuName = "Attacks/Normal Attacks/MeleeAttackBehavior")]
    public class MeleeAttackBehavior : ScriptableObject, INormalAttackBehavior
    {
        [SerializeField] private float _duration = 0.2f;

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
                if (_context.isGrounded)
                {
                    return Status.Success;
                }
                return Status.Fail;
            }
        }

        public void Behave()
        {
            _context.hitboxCollider.enabled = true;
            _context.AnimatorController.ToggleAttacking(true);
            Observable.FromCoroutine(_ => AttackTime()).Subscribe();
        }

        public void EndBehavior()
        {
            _context.hitboxCollider.enabled = false;
            _context.AnimatorController.ToggleAttacking(false);
        }

        IEnumerator AttackTime()
        {
            yield return new WaitForSeconds(_duration);
            _context.OnAttackEnded?.Invoke();
        }
    }
}