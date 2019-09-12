using UnityEngine;

namespace AlirezaTarahomi.FightingGame.InputSystem
{
    public interface InputManager
    {
        bool IsDown(string action);

        bool IsHeld(string action);

        bool IsReleased(string action);

        float GetAxis(string axis);
    }
}