using AlirezaTarahomi.FightingGame.Character.Event;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Powerup
{
    public class CharacterPowerupContext
    {
        public OnPowerupToggled OnPowerupToggled { get; private set; } = new();
    }
}