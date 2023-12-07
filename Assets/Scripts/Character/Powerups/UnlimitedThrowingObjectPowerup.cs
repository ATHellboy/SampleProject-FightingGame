using AlirezaTarahomi.FightingGame.General.Variable;
using System.Collections;
using UniRx;
using UnityEngine;
using VContainer;

namespace AlirezaTarahomi.FightingGame.Character.Powerup
{
    [CreateAssetMenu(menuName = "Custom/Powerups/UnlimitedThrowingObjectPowerup")]
    public class UnlimitedThrowingObjectPowerup : ScriptableObject, IPowerup
    {
        [SerializeField] private IntVariable _objectCounter;
        [SerializeField] private float _time = 2.5f;
        public float Time { get => _time; }
        [SerializeField] private float _cooldown = 1.0f;
        public float Cooldown { get => _cooldown; }

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
            Observable.FromCoroutine(_ => DisableTimer()).Subscribe().AddTo(_context.Disposables);
            return PowerupType.TimeBased;
        }

        IEnumerator DisableTimer()
        {
            yield return new WaitForSeconds(_time);
            _objectCounter.value = _lastObjectNumber;
            _context.OnPowerupEnded?.Invoke();
        }
    }
}