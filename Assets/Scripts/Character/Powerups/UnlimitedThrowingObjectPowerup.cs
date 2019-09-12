using AlirezaTarahomi.FightingGame.Character.Behavior;
using AlirezaTarahomi.FightingGame.Character.Behavior.Powerup;
using AlirezaTarahomi.FightingGame.Character.Event;
using Assets.Infrastructure.Scripts.CQRS;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Character.Powerup
{
    [CreateAssetMenu(menuName = "Powerups/UnlimitedThrowingObjectPowerup")]
    public class UnlimitedThrowingObjectPowerup : ScriptableObject, IPowerup, IThrowingBehavior
    {
        public float time = 5;

        public ScriptableObject PowerupAttackBehavior { get; }
        public ThrowingObjectBehavior ThrowingObjectBehavior { get; private set; }

        private const int INFINITE = 99999;

        private IMessageBus _messageBus;
        private CharacterPowerupContext _context;
        private int _lastObjectNumber;

        [Inject]
        public void Contruct(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public void Inject(CharacterPowerupContext context)
        {
            _context = context;
        }

        public void AssignThrowingObjectBehavior(ThrowingObjectBehavior throwingObjectBehavior)
        {
            ThrowingObjectBehavior = throwingObjectBehavior;
        }

        public PowerType Active()
        {
            _lastObjectNumber = ThrowingObjectBehavior.counter;
            ThrowingObjectBehavior.counter = INFINITE;
            Observable.FromCoroutine(_ => Timer()).Subscribe();
            _messageBus.RaiseEvent(new OnPowerupToggled(_context.PlayerId, _context.Stats, true));
            return PowerType.TimeBased;
        }

        public void Disable()
        {
            ThrowingObjectBehavior.counter = _lastObjectNumber;
            _messageBus.RaiseEvent(new OnPowerupToggled(_context.PlayerId, _context.Stats, false));
        }

        IEnumerator Timer()
        {
            yield return new WaitForSeconds(time);
            Disable();
        }
    }
}