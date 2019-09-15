using AlirezaTarahomi.FightingGame.Character.Behavior;
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
        [SerializeField] private float _time = 2.5f;

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
            _messageBus.RaiseEvent(new OnPowerupToggled(_context.CharacterId, true));
            return PowerType.TimeBased;
        }

        public void Disable()
        {
            ThrowingObjectBehavior.counter = _lastObjectNumber;
            _messageBus.RaiseEvent(new OnPowerupToggled(_context.CharacterId, false));
        }

        IEnumerator Timer()
        {
            yield return new WaitForSeconds(_time);
            Disable();
        }
    }
}