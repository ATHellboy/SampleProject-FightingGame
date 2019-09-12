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

        public Status Behave()
        {
            if (_context.isGrounded)
            {
                _context.hitboxCollider.enabled = true;
                _context.AnimatorController.ToggleAttacking(true);
                Observable.FromCoroutine(_ => AttackTime()).Subscribe();
                return Status.Success;
            }
            return Status.Fail;
        }

        public Status EndBehavior()
        {
            _context.hitboxCollider.enabled = false;
            _context.AnimatorController.ToggleAttacking(false);
            return Status.Success;
        }

        IEnumerator AttackTime()
        {
            yield return new WaitForSeconds(0.2f);
            _messageBus.RaiseEvent(new OnAttackToggled(_context.PlayerId, _context.Stats, false));
        }
    }
}