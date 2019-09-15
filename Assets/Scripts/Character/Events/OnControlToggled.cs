using Assets.Infrastructure.Scripts.CQRS;
using System;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Player.Event
{
    public class OnControlToggled : IEvent
    {
        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }
        public bool Enable { get; }

        public OnControlToggled(string entityId, bool enable)
        {
            EntityId = entityId;
            Enable = enable;
        }
    }
}