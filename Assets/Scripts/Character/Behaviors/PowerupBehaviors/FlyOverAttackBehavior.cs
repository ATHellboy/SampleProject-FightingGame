using AlirezaTarahomi.FightingGame.Character.Event;
using AlirezaTarahomi.FightingGame.Character.Powerup;
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
        public float velocity = 50;

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
            if (_context.isPowerupActive)
            {
                _messageBus.RaiseEvent(new OnControlToggled(_context.PlayerId, _context.Stats, false));
                _context.hitboxCollider.enabled = true;
                Observable.FromCoroutine(_ => FlyOver()).Subscribe();
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

        IEnumerator FlyOver()
        {
            _context.LocomotionHandler.PushForward(velocity);
            _context.AnimatorController.ToggleAttacking(true);

            while (_context.LocomotionHandler.GetVelocity().x != 0)
            {
                yield return null;
            }

            _messageBus.RaiseEvent(new OnPushForwardEnded(_context.PlayerId, _context.Stats));
            _messageBus.RaiseEvent(new OnAttackToggled(_context.PlayerId, _context.Stats, false));
        }
    }
}