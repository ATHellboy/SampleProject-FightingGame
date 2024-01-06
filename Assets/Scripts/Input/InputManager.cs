using UnityEngine;

namespace AlirezaTarahomi.FightingGame.InputSystem
{
    public interface InputManager
    {
        public enum Type { Jump, Attack, PowerupAttack }

        bool IsPressed(string action);

        bool IsHeld(string action);

        bool IsReleased(string action);

        float GetAxis(string axis);

        float GetAxisRaw(string axis);
    }
}