using UnityEngine;

namespace AlirezaTarahomi.FightingGame.InputSystem
{
    public class UnityInputManager : InputManager
    {
        public bool IsPressed(string action)
        {
            return Input.GetButtonDown(action);
        }

        public bool IsHeld(string action)
        {
            return Input.GetButton(action);
        }

        public bool IsReleased(string action)
        {
            return Input.GetButtonUp(action);
        }

        public float GetAxis(string axis)
        {
            return Input.GetAxis(axis);
        }
    }
}