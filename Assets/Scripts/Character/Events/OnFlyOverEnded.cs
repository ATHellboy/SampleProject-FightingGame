using Assets.Infrastructure.Scripts.CQRS;
using System;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Event
{
    public class OnFlyOverEnded : IEvent, ICharacterIdProperty
    {
        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }
        public string CharacterId { get; }

        public OnFlyOverEnded(string entityId)
        {
            EntityId = entityId;
            CharacterId = entityId;
        }
    }
}