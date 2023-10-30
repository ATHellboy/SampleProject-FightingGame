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

        public float Time { get => 0; }

        [SerializeField] private float _cooldown = 1.0f;
        public float Cooldown { get => _cooldown; }

        public PowerupType Active()
        {
            return PowerupType.OneTime;
        }
    }
}