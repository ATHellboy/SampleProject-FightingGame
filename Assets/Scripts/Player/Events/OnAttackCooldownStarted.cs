using System;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Player.Event
{
    public class OnAttackCooldownStarted
    {
        public Type attackType;
        public float cooldown;

        public OnAttackCooldownStarted(Type attackType, float cooldown)
        {
            this.attackType = attackType;
            this.cooldown = cooldown;
        }
    }
}