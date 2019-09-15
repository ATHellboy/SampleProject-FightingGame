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
        [SerializeField] private float _velocity = 50;
        [SerializeField] private float _distance = 8;

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
                if (!_context.isGrounded)
                {
                    return Status.Success;
                }
                return Status.Fail;
            }
        }

        public void Behave()
        {
            _messageBus.RaiseEvent(new OnCharacterFlownToggled(_context.CharacterId, true));
            _messageBus.RaiseEvent(new OnControlToggled(_context.CharacterId, false));
            _context.hitboxCollider.enabled = true;
            _context.AnimatorController.ToggleAttacking(true);
            Observable.FromCoroutine(_ => MoveForwardAndAttack()).Subscribe();
        }

        public void EndBehavior()
        {
            _messageBus.RaiseEvent(new OnCharacterFlownToggled(_context.CharacterId, false));
            _messageBus.RaiseEvent(new OnControlToggled(_context.CharacterId, true));
            _context.hitboxCollider.enabled = false;
            _context.AnimatorController.ToggleAttacking(false);
        }

        IEnumerator MoveForwardAndAttack()
        {
            _context.LocomotionHandler.PushForward(_velocity);

            yield return new WaitForSeconds(_distance / _velocity);

            _messageBus.RaiseEvent(new OnAttackToggled(_context.CharacterId, _context.PlayerId, false));
        }
    }
}