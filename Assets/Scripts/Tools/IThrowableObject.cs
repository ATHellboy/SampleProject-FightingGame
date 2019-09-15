using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Tool
{
    public interface IThrowableObject : ITool
    {
        bool CanPick { get; }

        void PrepareForPowerup();

        void Throw(float force, Vector2 direction);
    }
}