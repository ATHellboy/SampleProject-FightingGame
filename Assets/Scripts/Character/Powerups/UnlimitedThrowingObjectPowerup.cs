using AlirezaTarahomi.FightingGame.Character.Behavior;
using System.Collections;
using UniRx;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Powerup
{
    [CreateAssetMenu(menuName = "Powerups/UnlimitedThrowingObjectPowerup")]
    public class UnlimitedThrowingObjectPowerup : ScriptableObject, IPowerup, IThrowingBehavior
    {
        [SerializeField] private float _time = 2.5f;

        public ScriptableObject PowerupAttackBehavior { get; }
        public ThrowingObjectBehavior ThrowingObjectBehavior { get; private set; }

        private const int INFINITE = 99999;

        private CharacterPowerupContext _context;
        private int _lastObjectNumber;

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
            _context.OnPowerupToggled?.Invoke(true);
            return PowerType.TimeBased;
        }

        public void Disable()
        {
            ThrowingObjectBehavior.counter = _lastObjectNumber;
            _context.OnPowerupToggled?.Invoke(false);
        }

        IEnumerator Timer()
        {
            yield return new WaitForSeconds(_time);
            Disable();
        }
    }
}