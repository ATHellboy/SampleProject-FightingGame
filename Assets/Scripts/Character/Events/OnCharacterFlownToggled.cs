using Assets.Infrastructure.Scripts.CQRS;
using System;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Event
{
    public class OnCharacterFlownToggled : IEvent
    {
        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }
        public bool Enable { get; }

        public OnCharacterFlownToggled(string entityId, bool enable)
        {
            EntityId = entityId;
            Enable = enable;
        }
    }
}