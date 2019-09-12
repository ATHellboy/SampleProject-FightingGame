using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Powerup
{
    public interface IPowerup
    {
        ScriptableObject PowerupAttackBehavior { get; }

        PowerType Active();

        void Disable();

        void Inject(CharacterPowerupContext context);
    }

    public enum PowerType
    {
        OneTime,
        TimeBased
    }
}