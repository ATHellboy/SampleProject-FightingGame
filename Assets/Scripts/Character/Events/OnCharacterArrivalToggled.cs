using Assets.Infrastructure.Scripts.CQRS;
using System;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Event
{
    public class OnCharacterArrivalToggled : IEvent
    {
        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }
        public bool Enter { get; }

        public OnCharacterArrivalToggled(string entityId, bool enter)
        {
            EntityId = entityId;
            Enter = enter;
        }
    }
}