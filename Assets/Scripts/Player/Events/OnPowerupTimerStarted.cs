using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Player.Event
{
    public class OnPowerupTimerStarted
    {
        public float duration;
        public float cooldown;

        public OnPowerupTimerStarted(float duration, float cooldown)
        {
            this.duration = duration;
            this.cooldown = cooldown;
        }
    }
}
