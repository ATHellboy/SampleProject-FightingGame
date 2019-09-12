using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Character.Powerup
{
    public class CharacterPowerupContext
    {
        public CharacterStats Stats { get; private set; }
        public int PlayerId { get; private set; }

        public CharacterPowerupContext([Inject(Id = "stats")] CharacterStats stats,
            [Inject(Id = "id")] int playerId)
        {
            Stats = stats;
            PlayerId = playerId;
        }
    }
}