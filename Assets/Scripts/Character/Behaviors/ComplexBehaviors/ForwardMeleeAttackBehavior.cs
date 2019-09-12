using AlirezaTarahomi.FightingGame.Character.Event;
using AlirezaTarahomi.FightingGame.Player.Event;
using Assets.Infrastructure.Scripts.CQRS;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Character.Behavior.Complex
{
    [CreateAssetMenu(menuName = "Attacks/Complex Attacks/ForwardMeleeAttackBehavior")]
    public class ForwardMeleeAttackBehavior : ScriptableObject, IComplexAttackBehavior
    {
        public float velocity = 50;
        public float distance = 8;

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
            if (!_context.isGrounded)
            {
                _messageBus.RaiseEvent(new OnControlToggled(_context.PlayerId, _context.Stats, false));
                _context.hitboxCollider.enabled = true;
                _context.AnimatorController.ToggleAttacking(true);
                 Observable.FromCoroutine(_ => MoveForwardThenAttack()).Subscribe();
                return Status.Success;
            }
            return Status.Fail;
        }

        public Status EndBehavior()
        {
            _messageBus.RaiseEvent(new OnControlToggled(_context.PlayerId, _context.Stats, true));
            _context.hitboxCollider.enabled = false;
            _context.AnimatorController.ToggleAttacking(false);
            return Status.Success;
        }

        IEnumerator MoveForwardThenAttack()
        {
            _context.LocomotionHandler.PushForward(velocity);

            yield return new WaitForSeconds(distance / velocity);

            _messageBus.RaiseEvent(new OnPushForwardEnded(_context.PlayerId, _context.Stats));
            _messageBus.RaiseEvent(new OnAttackToggled(_context.PlayerId, _context.Stats, false));
        }
    }
}