using AlirezaTarahomi.FightingGame.General.Variable;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Tool
{
    public interface IThrowableObject : ITool
    {
        bool CanPick { get; }

        IntVariable ObjectCounter { get; }

        void PrepareForPowerup();

        void Throw(float force, Vector2 direction);
    }
}