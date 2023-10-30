using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Powerup
{
    public interface IPowerup
    {
        ScriptableObject PowerupAttackBehavior { get; }

        float Time { get; }

        float Cooldown { get; }

        PowerupType Active();
    }

    public enum PowerupType
    {
        OneTime,
        TimeBased
    }
}