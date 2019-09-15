using Assets.Infrastructure.Scripts.CQRS;
using System;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Character.Event
{
    public class OnGrounded : IEvent
    {
        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }

        public OnGrounded(string entityId)
        {
            EntityId = entityId;
        }
    }
}