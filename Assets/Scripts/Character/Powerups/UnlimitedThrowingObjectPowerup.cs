using AlirezaTarahomi.FightingGame.General.Variable;
using System.Collections;
using UniRx;
using UnityEngine;
using VContainer;

namespace AlirezaTarahomi.FightingGame.Character.Powerup
{
    [CreateAssetMenu(menuName = "Powerups/UnlimitedThrowingObjectPowerup")]
    public class UnlimitedThrowingObjectPowerup : ScriptableObject, IPowerup
    {
        [SerializeField] private IntVariable _objectCounter;
        [SerializeField] private float _time = 2.5f;
        [SerializeField] private float _powerupCooldown = 1.0f;
        public float PowerupCooldown { get => _powerupCooldown; }

        public ScriptableObject PowerupAttackBehavior { get; }

        private const int INFINITE = 99999;

        private CharacterPowerupContext _context;
        private int _lastObjectNumber;

        [Inject]
        public void Construct(CharacterPowerupContext context)
        {
            _context = context;
        }

        public PowerupType Active()
        {
            _lastObjectNumber = _objectCounter.value;
            _objectCounter.value = INFINITE;
            Observable.FromCoroutine(_ => Timer()).Subscribe();
            _context.OnPowerupToggled?.Invoke(true);
            return PowerupType.TimeBased;
        }

        public void Disable()
        {
            _objectCounter.value = _lastObjectNumber;
            _context.OnPowerupToggled?.Invoke(false);
        }

        IEnumerator Timer()
        {
            yield return new WaitForSeconds(_time);
            Disable();
        }
    }
}