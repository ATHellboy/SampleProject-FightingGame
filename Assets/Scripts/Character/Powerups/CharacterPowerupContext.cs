using UnityEngine;
using Zenject;

namespace AlirezaTarahomi.FightingGame.Character.Powerup
{
    public class CharacterPowerupContext
    {
        public string CharacterId { get; private set; }

        public CharacterPowerupContext([Inject(Id = "id")] string characterId)
        {
            CharacterId = characterId;
        }
    }
}