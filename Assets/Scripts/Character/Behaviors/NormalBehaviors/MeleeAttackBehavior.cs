using UnityEngine;
using Zenject;
using System.Collections;
using Assets.Infrastructure.Scripts.CQRS;
using AlirezaTarahomi.FightingGame.Character.Event;
using UniRx;

namespace AlirezaTarahomi.FightingGame.Character.Behavior.Normal
{
    [CreateAssetMenu(menuName = "Attacks/Normal Attacks/MeleeAttackBehavior")]
    public class MeleeAttackBehavior : ScriptableObject, INormalAttackBehavior
    {
        [SerializeField] private float _duration = 0.2f;

        private IMessageBus _messageBus;
        private CharacterBehaviorContext _context;

        [Inject]
        public void Construct(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public void Inject(CharacterBehaviorContext context)
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
            _messageBus.RaiseEvent(new OnAttackToggled(_context.CharacterId, _context.PlayerId, false));
        }
    }
}