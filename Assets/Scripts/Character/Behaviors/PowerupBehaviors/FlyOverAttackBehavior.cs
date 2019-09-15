using AlirezaTarahomi.FightingGame.Character.Event;
using AlirezaTarahomi.FightingGame.Player.Event;
using Assets.Infrastructure.Scripts.CQRS;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Character.Behavior.Powerup
{
    [CreateAssetMenu(menuName = "Attacks/Powerup Attacks/FlyOverAttackBehavior")]
    public class FlyOverAttackBehavior : ScriptableObject, IPowerupAttackBehavior
    {
        [SerializeField] private float _velocity = 50;

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
                if (_context.isPowerupActive)
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
            Observable.FromCoroutine(_ => FlyOver()).Subscribe();
        }

        public void EndBehavior()
        {
            _messageBus.RaiseEvent(new OnCharacterFlownToggled(_context.CharacterId, false));
            _messageBus.RaiseEvent(new OnControlToggled(_context.CharacterId, true));
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

            _messageBus.RaiseEvent(new OnFlyOverEnded(_context.CharacterId));
            _messageBus.RaiseEvent(new OnAttackToggled(_context.CharacterId, _context.PlayerId, false));
        }
    }
}