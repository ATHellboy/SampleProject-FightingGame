using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Powerup
{
    public interface IPowerup
    {
        ScriptableObject PowerupAttackBehavior { get; }

        float PowerupCooldown { get; }

        PowerupType Active();

        void Disable();

        void Inject(CharacterPowerupContext context);
    }

    public enum PowerupType
    {
        OneTime,
        TimeBased
    }
}