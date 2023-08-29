using AlirezaTarahomi.FightingGame.Character.Behavior.Powerup;
using ScriptableObjectDropdown;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Powerup
{
    [CreateAssetMenu(menuName = "Powerups/FlyOverPowerup")]
    public class FlyOverPowerup : ScriptableObject, IPowerup
    {
        [ScriptableObjectDropdown(typeof(FlyOverAttackBehavior))]
        public ScriptableObjectReference _powerupAttackBehavior;
        public ScriptableObject PowerupAttackBehavior { get { return _powerupAttackBehavior.value; } }

        private CharacterPowerupContext _context;

        public void Inject(CharacterPowerupContext context)
        {
            _context = context;
        }

        public PowerType Active()
        {
            _context.OnPowerupToggled?.Invoke(true);
            return PowerType.OneTime;
        }

        public void Disable()
        {
            _context.OnPowerupToggled?.Invoke(false);
        }
    }
}