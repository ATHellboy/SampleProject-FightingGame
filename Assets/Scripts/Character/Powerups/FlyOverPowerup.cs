using AlirezaTarahomi.FightingGame.Character.Behavior.Powerup;
using ScriptableObjectDropdown;
using UnityEngine;
using VContainer;

namespace AlirezaTarahomi.FightingGame.Character.Powerup
{
    [CreateAssetMenu(menuName = "Powerups/FlyOverPowerup")]
    public class FlyOverPowerup : ScriptableObject, IPowerup
    {
        [ScriptableObjectDropdown(typeof(FlyOverAttackBehavior))]
        public ScriptableObjectReference _powerupAttackBehavior;
        public ScriptableObject PowerupAttackBehavior { get { return _powerupAttackBehavior.value; } }

        [SerializeField] private float _powerupCooldown = 1.0f;
        public float PowerupCooldown { get => _powerupCooldown; }

        private CharacterPowerupContext _context;

        [Inject]
        public void Construct(CharacterPowerupContext context)
        {
            _context = context;
        }

        public PowerupType Active()
        {
            _context.OnPowerupToggled?.Invoke(true);
            return PowerupType.OneTime;
        }

        public void Disable()
        {
            _context.OnPowerupToggled?.Invoke(false);
        }
    }
}